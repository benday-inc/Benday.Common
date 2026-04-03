using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Benday.Common.Interfaces
{
    /// <summary>
    /// Base async service layer contract.
    /// Service layers compose repositories and add business logic/validation.
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
    /// Async service layer for owned/multi-tenant entities.
    /// </summary>
    public interface IAsyncOwnedItemService<T, TKey>
        : IAsyncService<T, TKey>
        where T : IOwnedItem<TKey>
        where TKey : IEquatable<TKey>
    {
        Task<T?> GetByIdAsync(string ownerId, TKey id);
        Task<IList<T>> GetByOwnerAsync(string ownerId);
    }
}
