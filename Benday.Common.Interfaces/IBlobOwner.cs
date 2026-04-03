namespace Benday.Common.Interfaces
{
    /// <summary>
    /// Entity that has blob attachments in Azure Storage.
    /// The blob infrastructure only needs the prefix string.
    /// How the prefix is constructed is the entity's concern.
    /// </summary>
    public interface IBlobOwner
    {
        string GetBlobPrefix();
    }
}
