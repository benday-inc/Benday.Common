using System;

namespace Benday.Common.Interfaces
{
    /// <summary>
    /// Entity that belongs to a tenant. TenantId is always a string
    /// regardless of the entity's key type.
    /// </summary>
    public interface ITenantItem<TKey> : IEntityIdentity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets the identifier of the tenant that owns this entity.
        /// </summary>
        string TenantId { get; set; }
    }
}
