using System;
using System.Collections.Generic;
using System.Linq;
using Timer = System.Timers.Timer;

namespace Spinnerino
{
    public class IndefiniteSpinner : IDisposable
    {
        private static readonly char[] DefaultChars = @"/-\|".ToCharArray();
        private readonly char[] _chars;
        private readonly bool _newlineWhenDone;
        private readonly Timer _timer = new Timer(65);
        private readonly int _initialCursorLeft = Console.CursorLeft;
        private readonly int _cursorLeft;
        private object _consoleLock;

        /// <summary>
        /// Constructs the spinner. Optionally by setting <paramref name="newlineWhenDone"/> to true the spinner will go to a new line when it is done.
        /// The sequence of animated characters can be customized by setting <paramref name="animationCharacters"/> - default is the classic /-|\ sequence.
        /// </summary>
        public IndefiniteSpinner(bool newlineWhenDone = true, IEnumerable<char> animationCharacters = null, object consoleLock = null)
        {
            _newlineWhenDone = newlineWhenDone;
            _chars = (animationCharacters ?? DefaultChars).ToArray();
            _consoleLock = consoleLock;

            if (_consoleLock == null)
            {
                _consoleLock = new object();
            }

            var characterIndex = 0;

            _timer.Elapsed += (o, ea) =>
            {
                var c = _chars[characterIndex % _chars.Length];

                lock (consoleLock)
                {
                    Print(c);
                }

                characterIndex++;
            };

            _timer.Start();
        }

        public void Dispose()
        {
            _timer.Dispose();

            Print(' ', newline: true);
        }

        private void Print(char c, bool newline = false)
        {
            Console.CursorLeft = _initialCursorLeft;
            Console.Write($"{c}");

            if (newline && _newlineWhenDone)
            {
                Console.WriteLine();
            }
        }
    }
}