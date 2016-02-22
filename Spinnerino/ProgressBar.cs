﻿using System;
using System.Collections.Generic;
using System.Threading;
using Timer = System.Timers.Timer;

namespace Spinnerino
{
    public class ProgressBar : IDisposable, IProgressPercentageIndicator
    {
        readonly Timer _timer = new Timer(65);
        readonly char _completedChar;
        readonly char _notCompletedChar;
        readonly int _bufferWidth;
        readonly int _cursorTop;

        double _progress;

        public ProgressBar(char completedChar = '=', char notCompletedChar = '-')
        {
            _completedChar = completedChar;
            _notCompletedChar = notCompletedChar;
            if (Console.CursorLeft > 0)
            {
                Console.WriteLine();
            }

            _bufferWidth = Console.BufferWidth;
            _cursorTop = Console.CursorTop;

            _timer.Elapsed += (o, ea) =>
            {
                Print(_progress);
            };
            _timer.Start();
        }

        void Print(double progress)
        {
            Console.CursorTop = _cursorTop;
            Console.CursorLeft = 0;

            var lineLength = _bufferWidth - 2;
            var progressLength = (int)(lineLength * (progress / 100));
            var theRestLength = lineLength - progressLength;

            var characters = new List<char>();

            characters.Add('|');
            characters.AddRange(new string(_completedChar, progressLength));
            characters.AddRange(new string(_notCompletedChar, theRestLength));
            characters.Add('|');

            var label = $"| {progress:0.##} % |";
            var labelLeft = (int)((lineLength - label.Length) / 2.0 + 1);

            // only apply the percentage label if the window is pretty wide
            if (labelLeft > 20)
            {
                for (var charIndex = 0; charIndex < label.Length; charIndex++)
                {
                    var lineIndex = charIndex + labelLeft;

                    characters[lineIndex] = label[charIndex];
                }
            }

            Console.Write(new string(characters.ToArray()));
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