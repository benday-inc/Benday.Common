using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Benday.Common.Interfaces;

namespace Benday.Common.Testing
{
    /// <summary>
    /// In-memory implementation of IAsyncReadableRepository for testing
    /// service layers with single-key entities (non-owned).
    /// </summary>
    public class InMemoryReadableRepository<T, TKey>
        : IAsyncReadableRepository<T, TKey>
        where T : IEntityIdentity<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly Dictionary<TKey, T> _Store = new();

        public Task SaveAsync(T entity)
        {
            _Store[entity.Id] = entity;
            return Task.CompletedTask;
        }

        public Task<T?> GetByIdAsync(TKey id)
        {
            _Store.TryGetValue(id, out var entity);
            return Task.FromResult<T?>(entity);
        }

        public Task<IList<T>> GetAllAsync()
        {
            return Task.FromResult<IList<T>>(_Store.Values.ToList());
        }

        public Task DeleteAsync(T entity)
        {
            _Store.Remove(entity.Id);
            return Task.CompletedTask;
        }
    }
}
