using System;
using System.Threading;
using Timer = System.Timers.Timer;

namespace Spinnerino
{
    public class InlineProgressBar : IDisposable, IProgressPercentageIndicator
    {
        private readonly int _width;
        private readonly char _completedChar;
        private readonly char _notCompletedChar;
        private readonly Timer _timer = new Timer(65);
        private readonly int _cursorLeft;
        private double _progress;
        private object _consoleLock;

        public InlineProgressBar(int width = 10, char completedChar = '#', char notCompletedChar = '-', object consoleLock = null)
        {
            _width = width;
            _completedChar = completedChar;
            _notCompletedChar = notCompletedChar;
            _cursorLeft = Console.CursorLeft;
            _consoleLock = consoleLock;

            if (_consoleLock == null)
            {
                _consoleLock = new object();
            }

            _timer.Elapsed += (o, ea) =>
            {
                lock (consoleLock)
                {
                    Print(_progress);
                }
            };

            _timer.Start();
        }

        private void Print(double progress)
        {
            Console.CursorLeft = _cursorLeft;

            var lineLength = _width;
            var progressLength = (int)(progress / 100.0 * lineLength);
            var theRestLength = lineLength - progressLength;
            var progressLine = new string(_completedChar, progressLength);
            var theRestLine = new string(_notCompletedChar, theRestLength);

            Console.Write($"[{progressLine}{theRestLine}] {progress:0.##} %     ");
        }

        public void SetProgress(double percentage)
        {
            Interlocked.Exchange(ref _progress, Math.Min(100, Math.Max(percentage, 0)));
        }

        public void Dispose()
        {
            _timer.Dispose();

            Print(100);
            Console.WriteLine();
        }
    }
}