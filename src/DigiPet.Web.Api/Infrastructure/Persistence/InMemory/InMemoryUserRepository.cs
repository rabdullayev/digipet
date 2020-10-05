using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigiPet.Web.Api.Domain.Model;
using DigiPet.Web.Api.Infrastructure.Persistence.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace DigiPet.Web.Api.Infrastructure.Persistence.InMemory
{
    /// <summary>
    /// InMemory Repository to encapsulate <see cref="IMemoryCache"/>
    /// </summary>
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly SemaphoreSlim _cacheSemaphore = new SemaphoreSlim(1, 1);
        private const string Key = nameof(User);

        private readonly IMemoryCache _cache;

        public InMemoryUserRepository(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            try
            {
                await _cacheSemaphore.WaitAsync();

                var result = _cache.Get<List<User>>(Key);

                return result?.FirstOrDefault(x => x.Id == id);
            }
            finally
            {
                _cacheSemaphore.Release();
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var result = _cache.Get<List<User>>(Key);

            return await Task.FromResult(result);
        }

        public async Task InsertAsync(User entity)
        {
            try
            {
                await _cacheSemaphore.WaitAsync();

                var result = _cache.Get<List<User>>(Key);

                if (result == null)
                {
                    var list = new List<User> { entity };
                    _cache.Set(Key, list);
                }
                else
                {
                    result.Add(entity);
                }
            }
            finally
            {
                _cacheSemaphore.Release();
            }
        }

        public async Task Adopt(User user, Animal animal)
        {
            try
            {
                await _cacheSemaphore.WaitAsync();

                user.Animals.Add(animal);
            }
            finally
            {
                _cacheSemaphore.Release();
            }
        }
    }
}
