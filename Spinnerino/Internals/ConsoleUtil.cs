using System;
using System.Text;

namespace Spinnerino.Internals;

static class ConsoleUtil
{
    const int FallbackWidth = 80;

    /// <summary>
    /// Animations only make sense when we're writing to an actual console/terminal
    /// </summary>
    public static bool IsInteractive => !Console.IsOutputRedirected;

    /// <summary>
    /// Gets the number of characters that can be written on a line without the cursor wrapping to the next line.
    /// The last column is deliberately left alone, because writing to it makes some consoles wrap/scroll.
    /// </summary>
    public static int GetUsableLineWidth()
    {
        if (!IsInteractive) return FallbackWidth;

        try
        {
            var width = Console.WindowWidth;

            return width > 1 ? width - 1 : FallbackWidth;
        }
        catch (Exception)
        {
            return FallbackWidth;
        }
    }

    /// <summary>
    /// Limits the given percentage to [0;100], treating NaN as 0
    /// </summary>
    public static double ClampPercentage(double percentage) => double.IsNaN(percentage) ? 0 : Math.Clamp(percentage, 0, 100);

    /// <summary>
    /// Moves to the next line if the cursor is not at the beginning of a line already
    /// </summary>
    public static void EnsureBeginningOfLine()
    {
        if (!IsInteractive) return;

        // on Unix, the cursor position is obtained by querying the terminal via stdin, which doesn't work when
        // input is redirected
        if (!OperatingSystem.IsWindows() && Console.IsInputRedirected) return;

        try
        {
            if (Console.CursorLeft > 0)
            {
                Console.WriteLine();
            }
        }
        catch (Exception)
        {
            // can't tell where the cursor is - just carry on
        }
    }
}

/// <summary>
/// Repeatedly writes text at the same position on the current line, by backspacing over whatever was written
/// previously. This way, the cursor position never needs to be read, which can fail or be unreliable when the
/// console's input or output is redirected.
/// </summary>
sealed class InlineRegion
{
    int _length;

    public void Write(string text)
    {
        var builder = new StringBuilder(2 * _length + text.Length + 2);

        builder.Append('\b', _length);
        builder.Append(text);

        // erase leftovers if the new text is shorter than the previous
        var excess = _length - text.Length;

        if (excess > 0)
        {
            builder.Append(' ', excess);
            builder.Append('\b', excess);
        }

        _length = text.Length;

        Console.Write(builder.ToString());
    }
}
