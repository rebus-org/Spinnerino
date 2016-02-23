using System;
using System.Collections.Generic;
using Timer = System.Timers.Timer;

namespace Spinnerino
{
    public class IndefiniteProgressBar : IDisposable, IActionIndicator
    {
        public enum ProgressDirection
        {
            LeftToRight,
            RightToLeft
        }

        readonly Timer _timer = new Timer(35);
        readonly char _moverChar;
        readonly char _backgroundChar;
        readonly ProgressDirection _direction;
        readonly int _moverWidth;
        readonly int _bufferWidth;
        readonly int _cursorTop;

        string _action = "";

        public IndefiniteProgressBar(char moverChar = '=', char backgroundChar = '-', int? moverWidth = null, ProgressDirection direction = ProgressDirection.LeftToRight)
        {
            _moverChar = moverChar;
            _backgroundChar = backgroundChar;
            _direction = direction;
            _moverWidth = moverWidth ?? Console.BufferWidth / 2;

            if (Console.CursorLeft > 0)
            {
                Console.WriteLine();
            }

            _bufferWidth = Console.BufferWidth;
            _cursorTop = Console.CursorTop;

            var tick = 0;

            _timer.Elapsed += (o, ea) =>
            {
                Print(_action, tick);

                tick++;
            };
            _timer.Start();
        }

        public void SetAction(string description)
        {
            _action = description?.Trim() ?? "";
        }

        void Print(string action, int tick = -1)
        {
            Console.CursorTop = _cursorTop;
            Console.CursorLeft = 0;

            var lineLength = _bufferWidth - 2;

            var characters = new List<char>();

            characters.Add('|');

            if (tick >= 0)
            {
                var bar = new List<char>(new string(_backgroundChar, lineLength));

                for (var index = tick; index < tick + _moverWidth; index++)
                {
                    var barIndex = _direction == ProgressDirection.LeftToRight
                        ? index % bar.Count
                        : (bar.Count - index + bar.Count) % bar.Count;

                    bar[barIndex] = _moverChar;
                }

                characters.AddRange(bar);
            }
            else
            {
                characters.AddRange(new string(_backgroundChar, lineLength));
            }

            characters.Add('|');

            if (!string.IsNullOrWhiteSpace(action))
            {
                var label = $"| {action} |";
                var labelLeft = (int)((lineLength - label.Length) / 2.0 + 1);

                // only apply the percentage label if the window is pretty wide
                if (labelLeft > 10)
                {
                    for (var charIndex = 0; charIndex < label.Length; charIndex++)
                    {
                        var lineIndex = charIndex + labelLeft;

                        characters[lineIndex] = label[charIndex];
                    }
                }
            }

            Console.Write(new string(characters.ToArray()));
        }

        public void Dispose()
        {
            _timer.Dispose();

            Print("");

            Console.WriteLine();
        }
    }
}