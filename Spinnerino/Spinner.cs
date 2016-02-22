using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Timer = System.Timers.Timer;

namespace Spinnerino
{
    /// <summary>
    /// Creates a simple spinner with a percentage that looks like this: 
    /// / 14 %
    /// - 25 %
    /// | 99 %
    ///   100 %
    /// </summary>
    public class Spinner : IDisposable, IProgressPercentageIndicator
    {
        static readonly char[] DefaultChars = @"/-\|".ToCharArray();
        readonly char[] _chars;
        readonly bool _newlineWhenDone;
        readonly Timer _timer = new Timer(65);
        readonly int _initialCursorLeft = Console.CursorLeft;
        double _progress;

        /// <summary>
        /// Constructs the spinner. Optionally by setting <paramref name="newlineWhenDone"/> to true the spinner will go to a new line when it is done.
        /// The sequence of animated characters can be customized by setting <paramref name="animationCharacters"/> - default is the classic /-|\ sequence.
        /// </summary>
        public Spinner(bool newlineWhenDone = true, IEnumerable<char> animationCharacters = null)
        {
            _newlineWhenDone = newlineWhenDone;

            _chars = (animationCharacters ?? DefaultChars).ToArray();

            var characterIndex = 0;

            _timer.Elapsed += (o, ea) =>
            {
                var c = _chars[characterIndex % _chars.Length];

                Print(c, _progress);

                characterIndex++;
            };

            _timer.Start();
        }

        public void SetProgress(double percentage)
        {
            var limitedPercentage = Math.Max(Math.Min(100, percentage), 0);

            Interlocked.Exchange(ref _progress, limitedPercentage);
        }

        public void Dispose()
        {
            _timer.Dispose();

            Print(' ', 100, newline:true);
        }

        void Print(char c, double progress, bool newline = false)
        {
            Console.CursorLeft = _initialCursorLeft;
            Console.Write($"{c} {progress:0.##} %     ");

            if (newline && _newlineWhenDone)
            {
                Console.WriteLine();
            }
        }
    }
}
