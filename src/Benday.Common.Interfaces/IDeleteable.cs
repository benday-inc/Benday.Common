namespace Benday.Common.Interfaces
{
    /// <summary>
    /// Marks an entity as supporting soft delete.
    /// Used by adapter layers to track items removed from collections
    /// during save operations.
    /// </summary>
    public interface IDeleteable
    {
        /// <summary>
        /// Gets or sets a value indicating whether this entity is marked for deletion.
        /// </summary>
        bool IsMarkedForDelete { get; set; }
    }
}
