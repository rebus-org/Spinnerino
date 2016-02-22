namespace Spinnerino
{
    /// <summary>
    /// Spinners capable of indicating progress as a percentage should implement this
    /// </summary>
    public interface IProgressPercentageIndicator
    {
        /// <summary>
        /// Sets the progress to the specified percentage which should be a value between 0 and 100, both included.
        /// </summary>
        void SetProgress(double percentage);
    }
}