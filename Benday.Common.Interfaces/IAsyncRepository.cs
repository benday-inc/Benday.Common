using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Benday.Common.Interfaces
{
    /// <summary>
    /// Base async repository — write operations only.
    /// </summary>
    public interface IAsyncRepository<T, TKey>
        where T : IEntityIdentity<TKey>
        where TKey : IEquatable<TKey>
    {
        Task SaveAsync(T entity);
        Task DeleteAsync(T entity);
    }

    /// <summary>
    /// Async repository with read operations.
    /// For storage backends with single-key lookups.
    /// </summary>
    public interface IAsyncReadableRepository<T, TKey>
        : IAsyncRepository<T, TKey>
        where T : IEntityIdentity<TKey>
        where TKey : IEquatable<TKey>
    {
        Task<T?> GetByIdAsync(TKey id);
        Task<IList<T>> GetAllAsync();
    }

    /// <summary>
    /// Async repository for tenant-scoped entities.
    /// Lookup is by tenantId + entity id.
    /// </summary>
    public interface IAsyncTenantRepository<T, TKey>
        : IAsyncRepository<T, TKey>
        where T : ITenantItem<TKey>
        where TKey : IEquatable<TKey>
    {
        Task<T?> GetByIdAsync(string tenantId, TKey id);
        Task<IList<T>> GetByTenantAsync(string tenantId);
    }
}
