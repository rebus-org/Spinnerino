using System;
using System.Collections.Generic;
using System.Linq;
using Timer = System.Timers.Timer;

namespace Spinnerino
{
    public class IndefiniteSpinner : IDisposable
    {
        static readonly char[] DefaultChars = @"/-\|".ToCharArray();
        readonly char[] _chars;
        readonly bool _newlineWhenDone;
        readonly Timer _timer = new Timer(65);
        readonly int _initialCursorLeft = Console.CursorLeft;

        /// <summary>
        /// Constructs the spinner. Optionally by setting <paramref name="newlineWhenDone"/> to true the spinner will go to a new line when it is done.
        /// The sequence of animated characters can be customized by setting <paramref name="animationCharacters"/> - default is the classic /-|\ sequence.
        /// </summary>
        public IndefiniteSpinner(bool newlineWhenDone = true, IEnumerable<char> animationCharacters = null)
        {
            _newlineWhenDone = newlineWhenDone;

            _chars = (animationCharacters ?? DefaultChars).ToArray();

            var characterIndex = 0;

            _timer.Elapsed += (o, ea) =>
            {
                var c = _chars[characterIndex % _chars.Length];

                Print(c);

                characterIndex++;
            };

            _timer.Start();
        }

        public void Dispose()
        {
            _timer.Dispose();

            Print(' ', newline: true);
        }

        void Print(char c, bool newline = false)
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