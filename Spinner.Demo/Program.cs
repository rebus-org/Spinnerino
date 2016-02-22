using System;
using System.Linq;
using System.Threading;

namespace Spinner.Demo
{
    class Program
    {
        static void Main()
        {
            SeparateLines();

            Inline();
        }

        /// <summary>
        /// Demonstrates how the spinner can spin on its own line
        /// </summary>
        static void SeparateLines()
        {
            Console.WriteLine("Spinning on its own line...");

            using (var spinner = new Spinnerino.Spinner())
            {
                Enumerable.Range(0, 100)
                    .ToList()
                    .ForEach(percentage =>
                    {
                        for (var index = 0; index < 20; index++)
                        {
                            spinner.SetProgress(percentage);
                            Thread.Sleep(1);
                        }
                    });
            }

            Console.WriteLine("Done!");
        }

        /// <summary>
        /// Demonstrates how the spinner stays on the current line
        /// </summary>
        static void Inline()
        {
            Console.Write("Spinning on the current line... ");

            using (var spinner = new Spinnerino.Spinner(newlineWhenDone: false))
            {
                Enumerable.Range(0, 100)
                    .ToList()
                    .ForEach(percentage =>
                    {
                        for (var index = 0; index < 20; index++)
                        {
                            spinner.SetProgress(percentage);
                            Thread.Sleep(1);
                        }
                    });
            }

            Console.WriteLine("Done!");
        }
    }
}
