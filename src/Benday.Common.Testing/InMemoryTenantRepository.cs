using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Benday.Common.Interfaces;

namespace Benday.Common.Testing
{
    /// <summary>
    /// In-memory implementation of IAsyncTenantRepository for testing
    /// service layers without any storage emulator or connection string.
    /// </summary>
    public class InMemoryTenantRepository<T, TKey>
        : IAsyncTenantRepository<T, TKey>
        where T : ITenantItem<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly Dictionary<string, Dictionary<TKey, T>> _Store = new();

        /// <inheritdoc />
        public Task SaveAsync(T entity)
        {
            if (!_Store.ContainsKey(entity.TenantId))
            {
                _Store[entity.TenantId] = new Dictionary<TKey, T>();
            }

            _Store[entity.TenantId][entity.Id] = entity;
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<T?> GetByIdAsync(string tenantId, TKey id)
        {
            if (_Store.TryGetValue(tenantId, out var tenantStore) &&
                tenantStore.TryGetValue(id, out var entity))
            {
                return Task.FromResult<T?>(entity);
            }

            return Task.FromResult<T?>(default);
        }

        /// <inheritdoc />
        public Task<IList<T>> GetByTenantAsync(string tenantId)
        {
            if (_Store.TryGetValue(tenantId, out var tenantStore))
            {
                return Task.FromResult<IList<T>>(tenantStore.Values.ToList());
            }

            return Task.FromResult<IList<T>>(new List<T>());
        }

        /// <inheritdoc />
        public Task DeleteAsync(T entity)
        {
            if (_Store.TryGetValue(entity.TenantId, out var tenantStore))
            {
                tenantStore.Remove(entity.Id);
            }

            return Task.CompletedTask;
        }
    }
}
