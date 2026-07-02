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
        /// <summary>
        /// Inserts or updates the specified entity in the underlying storage.
        /// </summary>
        /// <param name="entity">The entity to save.</param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        Task SaveAsync(T entity);
        /// <summary>
        /// Deletes the specified entity from the underlying storage.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
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
        /// <summary>
        /// Gets the entity that has the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to retrieve.</param>
        /// <returns>A task whose result is the matching entity, or null if no entity has the specified identifier.</returns>
        Task<T?> GetByIdAsync(TKey id);
        /// <summary>
        /// Gets all entities from the underlying storage.
        /// </summary>
        /// <returns>A task whose result is a list of all entities.</returns>
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
        /// <summary>
        /// Gets the entity that has the specified identifier within the given tenant.
        /// </summary>
        /// <param name="tenantId">The identifier of the tenant that owns the entity.</param>
        /// <param name="id">The identifier of the entity to retrieve.</param>
        /// <returns>A task whose result is the matching entity, or null if no matching entity exists in the tenant.</returns>
        Task<T?> GetByIdAsync(string tenantId, TKey id);
        /// <summary>
        /// Gets all entities that belong to the specified tenant.
        /// </summary>
        /// <param name="tenantId">The identifier of the tenant whose entities are retrieved.</param>
        /// <returns>A task whose result is a list of the tenant's entities.</returns>
        Task<IList<T>> GetByTenantAsync(string tenantId);
    }
}
