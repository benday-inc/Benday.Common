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
        string TenantId { get; set; }
    }
}
