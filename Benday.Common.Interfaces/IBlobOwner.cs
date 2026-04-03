namespace Benday.Common.Interfaces
{
    /// <summary>
    /// Entity that has blob attachments in Azure Storage.
    /// The blob infrastructure only needs the prefix string to locate attachments.
    /// How the prefix is constructed is the entity's concern — it might use
    /// OwnerId, Id, or any other combination.
    ///
    /// Example implementations:
    ///   Cosmos/Table: $"{OwnerId}/{Id}/"
    ///   System entity: $"system/{Id}/"
    ///   EF Core entity: $"{OwnerId}/{Id}/" where Id is int.ToString()
    /// </summary>
    public interface IBlobOwner
    {
        string GetBlobPrefix();
    }
}
