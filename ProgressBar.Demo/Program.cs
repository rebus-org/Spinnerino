using System;
using System.Linq;
using System.Threading;
using Spinnerino;

namespace ProgressBar.Demo
{
    class Program
    {
        static void Main()
        {
            ShowProgressBar("Pretty standard...");
            ShowProgressBar("Thick...", completedChar: '#');
            ShowProgressBar("Even thicker...", notCompletedChar: ' ', completedChar: '@');
            ShowProgressBar("Stars..", notCompletedChar: ' ', completedChar: '*');
            ShowProgressBar("G&T....", notCompletedChar: '-', completedChar: '>');
        }

        static void ShowProgressBar(string text, char completedChar = '=', char notCompletedChar = '-')
        {
            Console.WriteLine(text);

            using (var bar = new Spinnerino.ProgressBar(completedChar, notCompletedChar))
            {
                Work(bar);
            }

            Console.WriteLine("Done!");
        }

        static void Work(IProgressPercentageIndicator bar)
        {
            var random = new Random(DateTime.Now.GetHashCode());

            Enumerable.Range(0, 100)
                .ToList()
                .ForEach(percentage =>
                {
                    bar.SetProgress(percentage);

                    Thread.Sleep(random.Next(30) + 30);
                });
        }
    }
}
