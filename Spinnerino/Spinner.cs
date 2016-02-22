using System;

namespace Spinnerino
{
    public class Spinner : IDisposable
    {
        public void SetProgress(double percentage)
        {
            Console.WriteLine($"progress: {percentage:0.00}");
        }

        public void Dispose()
        {
            
        }
    }
}
