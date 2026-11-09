using System;
using Spinnerino.Internals;

namespace Spinnerino;

/// <summary>
/// Creates a progress bar that spans the width of the console window, with a "mover" running across it continuously,
/// optionally with a label in the middle. Looks like this:
/// <code>
/// |-------------------=========| Processed 0,94 MB |==========-------------------|
/// </code>
/// The progress bar always starts on a new line.
/// </summary>
public class IndefiniteProgressBar : IDisposable, IActionIndicator
{
    /// <summary>
    /// Direction of the mover
    /// </summary>
    public enum ProgressDirection
    {
        /// <summary>
        /// The mover moves from left to right
        /// </summary>
        LeftToRight,

        /// <summary>
        /// The mover moves from right to left
        /// </summary>
        RightToLeft
    }

    readonly ConsoleAnimation _animation;
    readonly char _moverChar;
    readonly char _backgroundChar;
    readonly ProgressDirection _direction;
    readonly int? _moverWidth;

    volatile string _action = "";
    long _tick;

    /// <summary>
    /// Constructs the progress bar, using <paramref name="moverChar"/> and <paramref name="backgroundChar"/> to fill it.
    /// The width of the mover can be set with <paramref name="moverWidth"/> - by default, it is half the width of the bar.
    /// Pass a <paramref name="consoleLock"/> if other threads write to the console, and lock on it while doing so.
    /// </summary>
    public IndefiniteProgressBar(char moverChar = '=', char backgroundChar = '-', int? moverWidth = null, ProgressDirection direction = ProgressDirection.LeftToRight, object? consoleLock = null)
    {
        if (moverWidth is not null)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(moverWidth.Value, 1, nameof(moverWidth));
        }

        _moverChar = moverChar;
        _backgroundChar = backgroundChar;
        _moverWidth = moverWidth;
        _direction = direction;

        lock (consoleLock ?? new object())
        {
            ConsoleUtil.EnsureBeginningOfLine();
        }

        _animation = new ConsoleAnimation(TimeSpan.FromMilliseconds(35), consoleLock, RenderFrame, RenderFinalFrame);
    }

    /// <inheritdoc />
    public void SetAction(string description) => _action = description?.Trim() ?? "";

    /// <summary>
    /// Stops the progress bar and moves to the next line
    /// </summary>
    public void Dispose() => _animation.Dispose();

    void RenderFrame()
    {
        Console.Write($"\r{GetLine(_action, _tick)}");
        _tick++;
    }

    void RenderFinalFrame(bool interactive)
    {
        // when output is redirected, a line full of background characters is of no use to anyone
        if (!interactive) return;

        Console.Write($"\r{GetLine("", tick: null)}");
        Console.WriteLine();
    }

    string GetLine(string action, long? tick)
    {
        var barLength = Math.Max(ConsoleUtil.GetUsableLineWidth() - 2, 1);
        var bar = new string(_backgroundChar, barLength).ToCharArray();

        if (tick is { } t)
        {
            var moverWidth = Math.Min(_moverWidth ?? barLength / 2, barLength);

            for (var offset = 0; offset < moverWidth; offset++)
            {
                var position = t + offset;

                var index = _direction == ProgressDirection.LeftToRight
                    ? Modulo(position, barLength)
                    : Modulo(-position, barLength);

                bar[index] = _moverChar;
            }
        }

        if (!string.IsNullOrWhiteSpace(action))
        {
            // only apply the label if the window is pretty wide
            Bars.ApplyLabel(bar, $"| {action} |", minimumMargin: 10);
        }

        return $"|{new string(bar)}|";
    }

    static int Modulo(long value, int modulus) => (int)((value % modulus + modulus) % modulus);
}
