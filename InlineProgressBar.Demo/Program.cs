using System;
using System.Linq;
using System.Threading;

namespace InlineProgressBar.Demo
{
    class Program
    {
        static void Main()
        {
            Console.Write("Doing important stuff: ");

            using (var bar = new Spinnerino.InlineProgressBar())
            {
                Go(bar);
            }

            Console.Write("Another thing: ");

            using (var bar = new Spinnerino.InlineProgressBar(completedChar: '+', notCompletedChar: ' ', width: 20))
            {
                Go(bar);
            }
        }

        static void Go(Spinnerino.InlineProgressBar bar)
        {
            Enumerable.Range(0, 100)
                .ToList()
                .ForEach(percentage =>
                {
                    bar.SetProgress(percentage);

                    Thread.Sleep(20);
                });
        }
    }
}
