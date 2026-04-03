using System;

namespace Benday.Common.Interfaces
{
    /// <summary>
    /// Entity that belongs to an owner/tenant.
    /// OwnerId is always a string regardless of the entity's key type —
    /// the owner identifier comes from authentication and is always string-based.
    /// </summary>
    public interface IOwnedItem<TKey> : IEntityIdentity<TKey>
        where TKey : IEquatable<TKey>
    {
        string OwnerId { get; set; }
    }
}
