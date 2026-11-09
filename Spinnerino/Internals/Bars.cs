using System;

namespace Spinnerino.Internals;

static class Bars
{
    /// <summary>
    /// Writes the label centered on top of the bar, if it fits with at least <paramref name="minimumMargin"/> characters on each side
    /// </summary>
    public static void ApplyLabel(char[] bar, string label, int minimumMargin)
    {
        var labelLeft = (bar.Length - label.Length) / 2;

        if (labelLeft < minimumMargin) return;

        label.AsSpan().CopyTo(bar.AsSpan(labelLeft));
    }
}
