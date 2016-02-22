using System;
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
    public class Spinner : IDisposable
    {
        static readonly char[] Chars = { '/', '-', '\\', '|' };
        readonly Timer _timer = new Timer(100);
        double _progress;

        public Spinner()
        {
            var characterIndex = 0;

            _timer.Elapsed += (o, ea) =>
            {
                var c = Chars[characterIndex % Chars.Length];

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
            Console.CursorLeft = 0;
            Console.Write($"{c} {progress:0.##} %     ");

            if (newline)
            {
                Console.WriteLine();
            }
        }
    }
}
