using System.Threading;

namespace IndefiniteSpinner.Demo
{
    class Program
    {
        /// <summary>
        ///  awwwwww!!
        /// </summary>
        static readonly string[] SpinnerSequencesThatDontWorkInConsole =
        {
            "⣾⣽⣻⢿⡿⣟⣯⣷",
            "⠁⠂⠄⡀⢀⠠⠐⠈",
            "◐◓◑◒",
            "▖▘▝▗",
        };

        static readonly string[] SpinnerSequences =
        {
            "<^>v",
            ".oO°Oo.",
            @"/-\|", //< this one is default
            "..ooOO@@@@*",
        };

        static void Main()
        {
            foreach (var animationCharacters  in SpinnerSequences)
            {
                using (new Spinnerino.IndefiniteSpinner(animationCharacters: animationCharacters))
                {
                    Thread.Sleep(2000);
                }
            }
        }
    }
}
