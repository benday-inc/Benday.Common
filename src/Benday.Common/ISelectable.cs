namespace Benday.Common
{
    /// <summary>
    /// Utility interface for objects that can be selected .
    /// </summary>
    public interface ISelectable
    {
        /// <summary>
        /// Is the current object selected
        /// </summary>
        bool IsSelected { get; set; }
    }
}
