using System;

namespace Benday.Common.Interfaces
{
    /// <summary>
    /// Entity that has a parent entity. Extends ITenantItem because
    /// parented items always belong to a tenant.
    /// ParentId identifies the parent entity.
    /// </summary>
    public interface IParentedItem<TKey> : ITenantItem<TKey>
        where TKey : IEquatable<TKey>
    {
        string ParentId { get; set; }
    }
}
