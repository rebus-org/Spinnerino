using System.Linq;
using System.Threading;

namespace Spinnerino.ConsoleTest
{
    class Program
    {
        static void Main()
        {
            using (var spinner = new Spinner())
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
        }
    }
}
