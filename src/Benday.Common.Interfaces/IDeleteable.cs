namespace Benday.Common.Interfaces
{
    /// <summary>
    /// Marks an entity as supporting soft delete.
    /// Used by adapter layers to track items removed from collections
    /// during save operations.
    /// </summary>
    public interface IDeleteable
    {
        bool IsMarkedForDelete { get; set; }
    }
}
