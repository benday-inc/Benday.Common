using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Benday.Common.Interfaces;

namespace Benday.Common.Testing
{
    /// <summary>
    /// In-memory implementation of IAsyncOwnedItemRepository for testing
    /// service layers without any storage emulator or connection string.
    /// Uses a nested dictionary keyed by ownerId then entity id.
    /// </summary>
    public class InMemoryOwnedItemRepository<T, TKey>
        : IAsyncOwnedItemRepository<T, TKey>
        where T : IOwnedItem<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly Dictionary<string, Dictionary<TKey, T>> _Store = new();

        public Task SaveAsync(T entity)
        {
            if (!_Store.ContainsKey(entity.OwnerId))
            {
                _Store[entity.OwnerId] = new Dictionary<TKey, T>();
            }

            _Store[entity.OwnerId][entity.Id] = entity;
            return Task.CompletedTask;
        }

        public Task<T?> GetByIdAsync(string ownerId, TKey id)
        {
            if (_Store.TryGetValue(ownerId, out var ownerStore) &&
                ownerStore.TryGetValue(id, out var entity))
            {
                return Task.FromResult<T?>(entity);
            }

            return Task.FromResult<T?>(default);
        }

        public Task<IList<T>> GetByOwnerAsync(string ownerId)
        {
            if (_Store.TryGetValue(ownerId, out var ownerStore))
            {
                return Task.FromResult<IList<T>>(ownerStore.Values.ToList());
            }

            return Task.FromResult<IList<T>>(new List<T>());
        }

        public Task DeleteAsync(T entity)
        {
            if (_Store.TryGetValue(entity.OwnerId, out var ownerStore))
            {
                ownerStore.Remove(entity.Id);
            }

            return Task.CompletedTask;
        }
    }
}
