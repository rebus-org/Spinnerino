using System;
using System.Diagnostics;
using System.Threading;
using Spinnerino;

namespace IndefiniteProgressBar.Demo
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Default");

            using (var bar = new Spinnerino.IndefiniteProgressBar())
            {
                PretendToUploadSomething(bar, "Processed");
            }

            Console.WriteLine("Customized");

            using (var bar = new Spinnerino.IndefiniteProgressBar(moverChar: '>', backgroundChar: ' ', moverWidth: 5))
            {
                PretendToUploadSomething(bar, "Uploading");
            }

            Console.WriteLine("Customized");

            using (var bar = new Spinnerino.IndefiniteProgressBar(moverChar: '<', backgroundChar: ' ', moverWidth: 5, direction: Spinnerino.IndefiniteProgressBar.ProgressDirection.RightToLeft))
            {
                PretendToUploadSomething(bar, "Downloading");
            }
        }

        static void PretendToUploadSomething(IActionIndicator bar, string action)
        {
            var stopwatch = Stopwatch.StartNew();

            while (stopwatch.Elapsed < TimeSpan.FromSeconds(5))
            {
                bar.SetAction($"{action} {stopwatch.Elapsed.TotalSeconds:0.00} MB");

                Thread.Sleep(100);
            }
        }
    }
}
