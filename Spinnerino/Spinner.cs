using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Spinnerino.Internals;

namespace Spinnerino;

/// <summary>
/// Creates a simple spinner with a percentage that looks like this:
/// <code>
/// / 14 %
/// - 25 %
/// | 99 %
/// 100 %
/// </code>
/// The spinner is rendered at the current cursor position, so it can be used inline after some text.
/// </summary>
public class Spinner : IDisposable, IProgressPercentageIndicator
{
    static readonly char[] DefaultChars = @"/-\|".ToCharArray();

    readonly InlineRegion _region = new();
    readonly ConsoleAnimation _animation;
    readonly char[] _chars;
    readonly bool _newlineWhenDone;

    double _progress;
    int _characterIndex;

    /// <summary>
    /// Constructs the spinner. Optionally by setting <paramref name="newlineWhenDone"/> to true the spinner will go to a new line when it is done.
    /// The sequence of animated characters can be customized by setting <paramref name="animationCharacters"/> - default is the classic /-\| sequence.
    /// Pass a <paramref name="consoleLock"/> if other threads write to the console, and lock on it while doing so.
    /// </summary>
    public Spinner(bool newlineWhenDone = true, IEnumerable<char>? animationCharacters = null, object? consoleLock = null)
    {
        _newlineWhenDone = newlineWhenDone;
        _chars = (animationCharacters ?? DefaultChars).ToArray();

        if (_chars.Length == 0)
        {
            throw new ArgumentException("Please provide at least one animation character", nameof(animationCharacters));
        }

        _animation = new ConsoleAnimation(TimeSpan.FromMilliseconds(65), consoleLock, RenderFrame, RenderFinalFrame);
    }

    /// <inheritdoc />
    public void SetProgress(double percentage) => Interlocked.Exchange(ref _progress, ConsoleUtil.ClampPercentage(percentage));

    /// <summary>
    /// Stops the spinner and renders it as completed
    /// </summary>
    public void Dispose() => _animation.Dispose();

    void RenderFrame()
    {
        var c = _chars[_characterIndex];
        _characterIndex = (_characterIndex + 1) % _chars.Length;

        _region.Write($"{c} {Volatile.Read(ref _progress):0.##} %");
    }

    void RenderFinalFrame(bool interactive)
    {
        if (interactive)
        {
            _region.Write("100 %");
        }
        else
        {
            Console.Write("100 %");
        }

        if (_newlineWhenDone)
        {
            Console.WriteLine();
        }
    }
}
