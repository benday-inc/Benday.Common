namespace Benday.Common.Interfaces
{
    /// <summary>
    /// Entity that has blob attachments in Azure Storage.
    /// The blob infrastructure only needs the prefix string.
    /// How the prefix is constructed is the entity's concern.
    /// </summary>
    public interface IBlobOwner
    {
        /// <summary>
        /// Gets the prefix used to locate this entity's blobs in Azure Storage.
        /// </summary>
        /// <returns>The blob prefix string for this entity.</returns>
        string GetBlobPrefix();
    }
}
