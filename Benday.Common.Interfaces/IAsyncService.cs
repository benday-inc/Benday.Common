using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Benday.Common.Interfaces
{
    /// <summary>
    /// Base async service layer contract.
    /// </summary>
    public interface IAsyncService<T, TKey>
        where T : IEntityIdentity<TKey>
        where TKey : IEquatable<TKey>
    {
        Task SaveAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T?> GetByIdAsync(TKey id);
    }

    /// <summary>
    /// Async service layer for tenant-scoped entities.
    /// </summary>
    public interface IAsyncTenantService<T, TKey>
        : IAsyncService<T, TKey>
        where T : ITenantItem<TKey>
        where TKey : IEquatable<TKey>
    {
        Task<T?> GetByIdAsync(string tenantId, TKey id);
        Task<IList<T>> GetByTenantAsync(string tenantId);
    }
}
