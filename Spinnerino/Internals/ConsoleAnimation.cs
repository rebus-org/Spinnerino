using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Spinnerino.Internals;

/// <summary>
/// Drives an animation: renders a frame immediately and then periodically on a background task until disposed,
/// at which point the final frame is rendered. All console access happens while holding the console lock, and
/// no frame is ever rendered after the final one.
/// When console output is redirected, nothing is animated - only the final frame is rendered (with
/// <c>interactive: false</c>), so log files don't get filled with garbage.
/// </summary>
sealed class ConsoleAnimation : IDisposable
{
    readonly CancellationTokenSource _cancellationTokenSource = new();
    readonly object _consoleLock;
    readonly Action _renderFrame;
    readonly Action<bool> _renderFinalFrame;

    bool _done;

    public ConsoleAnimation(TimeSpan interval, object? consoleLock, Action renderFrame, Action<bool> renderFinalFrame)
    {
        _consoleLock = consoleLock ?? new object();
        _renderFrame = renderFrame;
        _renderFinalFrame = renderFinalFrame;

        IsInteractive = ConsoleUtil.IsInteractive;

        if (!IsInteractive) return;

        RenderFrame();

        _ = Task.Run(() => RunAsync(interval, _cancellationTokenSource.Token));
    }

    public bool IsInteractive { get; }

    async Task RunAsync(TimeSpan interval, CancellationToken cancellationToken)
    {
        using var timer = new PeriodicTimer(interval);

        try
        {
            while (await timer.WaitForNextTickAsync(cancellationToken).ConfigureAwait(false))
            {
                if (!RenderFrame()) return;
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            // we're done
        }
    }

    bool RenderFrame()
    {
        lock (_consoleLock)
        {
            if (_done) return false;

            try
            {
                _renderFrame();
                return true;
            }
            catch (Exception)
            {
                // the console probably went away or became unusable - there's no one to tell about it, and an
                // animation is not worth crashing anything over, so we just stop animating
                return false;
            }
        }
    }

    public void Dispose()
    {
        lock (_consoleLock)
        {
            if (_done) return;
            _done = true;

            _cancellationTokenSource.Cancel();

            try
            {
                _renderFinalFrame(IsInteractive);
            }
            catch (IOException)
            {
                // don't let a broken console throw from Dispose, because that would mask any exception thrown
                // from inside the using block
            }
        }

        _cancellationTokenSource.Dispose();
    }
}
