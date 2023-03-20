namespace Benday.Common
{
    /// <summary>
    /// Interface to indicate that an object is deletable. This is helpful for
    /// handling changes to collections of objects during CRUD operations. 
    /// </summary>
    public interface IDeleteable : IInt32Identity
    {
        /// <summary>
        /// Set this to true to indicate that this instance should be deleted.
        /// </summary>
        bool IsMarkedForDelete { get; set; }
    }
}
