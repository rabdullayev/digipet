using System.Collections.Generic;
using DigiPet.Web.Api.Domain.Model;
using DigiPet.Web.Api.Domain.Model.Metrics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace DigiPet.Web.Api.IntegrationTests.Helpers
{
    public class FakeWebApplicationFactory<T> : WebApplicationFactory<T>
        where T : class
    {
        public FakeWebApplicationFactory()
        {
            FakeUsersCache = new MemoryCache(new MemoryCacheOptions());
            ResetCache();
        }
        public IMemoryCache FakeUsersCache { get; private set; }
        public void ResetCache()
        {
            FakeUsersCache.Remove(nameof(User));
            var user1 = new User(1, "John");
            user1.Animals.Add(new Animal(1, 2, "cat", new Dictionary<MetricType, Metric>
            {
                { MetricType.Happiness, new Happiness(0.0f, 0.0f) }, 
                { MetricType.Hunger, new Hunger(0.0f, 0.0f) }
            }));
            user1.Animals.Add(new Animal(2, 3, "dog", new Dictionary<MetricType, Metric>()));
            var user2 = new User(2, "Wick");
            var user3 = new User(3, "Ben");
            FakeUsersCache.Set(nameof(User), new List<User>
            {
                user1,
                user2,
                user3
            });
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IMemoryCache>(FakeUsersCache);
            });
        }
    }
}
