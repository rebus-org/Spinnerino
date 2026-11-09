using System;
using System.Threading;
using Spinnerino.Internals;

namespace Spinnerino;

/// <summary>
/// Creates a progress bar that spans the width of the console window and looks like this:
/// <code>
/// |===================----------------| 25 % |-----------------------------------|
/// </code>
/// The progress bar always starts on a new line.
/// </summary>
public class ProgressBar : IDisposable, IProgressPercentageIndicator
{
    readonly ConsoleAnimation _animation;
    readonly char _completedChar;
    readonly char _notCompletedChar;

    double _progress;

    /// <summary>
    /// Constructs the progress bar, using <paramref name="completedChar"/> and <paramref name="notCompletedChar"/> to fill it.
    /// Pass a <paramref name="consoleLock"/> if other threads write to the console, and lock on it while doing so.
    /// </summary>
    public ProgressBar(char completedChar = '=', char notCompletedChar = '-', object? consoleLock = null)
    {
        _completedChar = completedChar;
        _notCompletedChar = notCompletedChar;

        lock (consoleLock ?? new object())
        {
            ConsoleUtil.EnsureBeginningOfLine();
        }

        _animation = new ConsoleAnimation(TimeSpan.FromMilliseconds(65), consoleLock,
            () => Console.Write($"\r{GetLine(Volatile.Read(ref _progress))}"),
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
        Console.Write(interactive ? $"\r{GetLine(100)}" : GetLine(100));
        Console.WriteLine();
    }

    string GetLine(double progress)
    {
        var barLength = Math.Max(ConsoleUtil.GetUsableLineWidth() - 2, 1);
        var progressLength = Math.Clamp((int)(barLength * (progress / 100)), 0, barLength);

        var bar = new char[barLength];
        bar.AsSpan(0, progressLength).Fill(_completedChar);
        bar.AsSpan(progressLength).Fill(_notCompletedChar);

        // only apply the percentage label if the window is pretty wide
        Bars.ApplyLabel(bar, $"| {progress:0.##} % |", minimumMargin: 20);

        return $"|{new string(bar)}|";
    }
}
