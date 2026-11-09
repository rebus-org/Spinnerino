using System;
using System.Threading;
using Spinnerino.Internals;

namespace Spinnerino;

/// <summary>
/// Creates a small progress bar at the current cursor position that looks like this:
/// <code>
/// [######----] 63 %
/// </code>
/// </summary>
public class InlineProgressBar : IDisposable, IProgressPercentageIndicator
{
    readonly InlineRegion _region = new();
    readonly ConsoleAnimation _animation;
    readonly int _width;
    readonly char _completedChar;
    readonly char _notCompletedChar;

    double _progress;

    /// <summary>
    /// Constructs the progress bar. The bar will be <paramref name="width"/> characters wide (not counting the brackets),
    /// using <paramref name="completedChar"/> and <paramref name="notCompletedChar"/> to fill it.
    /// Pass a <paramref name="consoleLock"/> if other threads write to the console, and lock on it while doing so.
    /// </summary>
    public InlineProgressBar(int width = 10, char completedChar = '#', char notCompletedChar = '-', object? consoleLock = null)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(width, 1);

        _width = width;
        _completedChar = completedChar;
        _notCompletedChar = notCompletedChar;

        _animation = new ConsoleAnimation(TimeSpan.FromMilliseconds(65), consoleLock,
            () => _region.Write(GetText(Volatile.Read(ref _progress))),
            RenderFinalFrame);
    }

    /// <inheritdoc />
    public void SetProgress(double percentage) => Interlocked.Exchange(ref _progress, ConsoleUtil.ClampPercentage(percentage));

    /// <summary>
    /// Stops the progress bar, renders it as completed, and moves to the next line
    /// </summary>
    public void Dispose() => _animation.Dispose();

    void RenderFinalFrame(bool interactive)
    {
        var text = GetText(100);

        if (interactive)
        {
            _region.Write(text);
        }
        else
        {
            Console.Write(text);
        }

        Console.WriteLine();
    }

    string GetText(double progress)
    {
        var progressLength = Math.Clamp((int)(progress / 100.0 * _width), 0, _width);
        var progressLine = new string(_completedChar, progressLength);
        var theRestLine = new string(_notCompletedChar, _width - progressLength);

        return $"[{progressLine}{theRestLine}] {progress:0.##} %";
    }
}
