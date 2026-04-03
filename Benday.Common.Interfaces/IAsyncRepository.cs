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
    /// For storage backends with single-key lookups (simple table storage, EF Core).
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
    /// Async repository for owned/multi-tenant entities.
    /// Lookup is by ownerId + entity id — matches the Cosmos DB partition key pattern
    /// and the Table Storage partitionKey + rowKey pattern.
    /// </summary>
    public interface IAsyncOwnedItemRepository<T, TKey>
        : IAsyncRepository<T, TKey>
        where T : IOwnedItem<TKey>
        where TKey : IEquatable<TKey>
    {
        Task<T?> GetByIdAsync(string ownerId, TKey id);
        Task<IList<T>> GetByOwnerAsync(string ownerId);
    }
}
