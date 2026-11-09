namespace Spinnerino;

/// <summary>
/// Indicators capable of showing a description of what is currently going on should implement this
/// </summary>
public interface IActionIndicator
{
    /// <summary>
    /// Sets the description of what is currently going on
    /// </summary>
    void SetAction(string description);
}
