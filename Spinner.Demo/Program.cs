using System;
using System.Linq;
using System.Threading;
using Spinnerino;

namespace Spinner.Demo
{
    class Program
    {
        static void Main()
        {
            SeparateLines();

            Inline();

            CustomCharacters();
        }

        /// <summary>
        /// Demonstrates how the spinner can spin on its own line
        /// </summary>
        static void SeparateLines()
        {
            Console.WriteLine("Spinning on its own line...");

            using (var spinner = new Spinnerino.Spinner())
            {
                SpinIt(spinner, 20);
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
                SpinIt(spinner, 20);
            }

            Console.WriteLine("Done!");
        }

        /// <summary>
        /// Demonstrates how the spinner can loop through a custom sequence of chatacters
        /// </summary>
        static void CustomCharacters()
        {
            Console.Write("Spinning custom chars... ");

            const string animationCharacters = "~^´`^+=-";

            using (var spinner = new Spinnerino.Spinner(newlineWhenDone: false, animationCharacters: animationCharacters))
            {
                SpinIt(spinner, 50);
            }

            Console.WriteLine("Done!");
        }

        static void SpinIt(IProgressPercentageIndicator spinner, int sleetMilliseconds)
        {
            Enumerable.Range(0, 100)
                .ToList()
                .ForEach(percentage =>
                {
                    spinner.SetProgress(percentage);

                    Thread.Sleep(sleetMilliseconds);
                });
        }
    }
}
