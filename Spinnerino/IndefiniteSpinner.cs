using System;
using System.Collections.Generic;
using System.Linq;
using Spinnerino.Internals;

namespace Spinnerino;

/// <summary>
/// Creates a simple spinner without any progress indication, rendered at the current cursor position. Useful when
/// you have no idea how long something is going to take.
/// </summary>
public class IndefiniteSpinner : IDisposable
{
    static readonly char[] DefaultChars = @"/-\|".ToCharArray();

    readonly InlineRegion _region = new();
    readonly ConsoleAnimation _animation;
    readonly char[] _chars;
    readonly bool _newlineWhenDone;

    int _characterIndex;

    /// <summary>
    /// Constructs the spinner. Optionally by setting <paramref name="newlineWhenDone"/> to true the spinner will go to a new line when it is done.
    /// The sequence of animated characters can be customized by setting <paramref name="animationCharacters"/> - default is the classic /-\| sequence.
    /// Pass a <paramref name="consoleLock"/> if other threads write to the console, and lock on it while doing so.
    /// </summary>
    public IndefiniteSpinner(bool newlineWhenDone = true, IEnumerable<char>? animationCharacters = null, object? consoleLock = null)
    {
        _newlineWhenDone = newlineWhenDone;
        _chars = (animationCharacters ?? DefaultChars).ToArray();

        if (_chars.Length == 0)
        {
            throw new ArgumentException("Please provide at least one animation character", nameof(animationCharacters));
        }

        _animation = new ConsoleAnimation(TimeSpan.FromMilliseconds(65), consoleLock, RenderFrame, RenderFinalFrame);
    }

    /// <summary>
    /// Stops the spinner and removes it
    /// </summary>
    public void Dispose() => _animation.Dispose();

    void RenderFrame()
    {
        var c = _chars[_characterIndex];
        _characterIndex = (_characterIndex + 1) % _chars.Length;

        _region.Write(c.ToString());
    }

    void RenderFinalFrame(bool interactive)
    {
        if (interactive)
        {
            _region.Write("");
        }

        if (_newlineWhenDone)
        {
            Console.WriteLine();
        }
    }
}
