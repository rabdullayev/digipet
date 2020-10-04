using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DigiPet.Web.Api.Application.Commands;
using DigiPet.Web.Api.Configs;
using DigiPet.Web.Api.Domain.Model;
using DigiPet.Web.Api.IntegrationTests.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace DigiPet.Web.Api.IntegrationTests.ApiTests
{
    public class UserAnimalsControllerTests : IClassFixture<FakeWebApplicationFactory<Startup>>, IDisposable
    {
        private const string GetUserAnimalsEndpoint = "/api/users/{0}/animals";
        private const string GetUserAnimalEndpoint = "/api/users/{0}/animals/{1}";
        private const string AdoptAnimalEndpoint = "/api/users/{0}/animals/adopt";
        private const string StrokeAnimalEndpoint = "/api/users/{0}/animals/stroke";
        private const string FeedAnimalEndpoint = "/api/users/{0}/animals/feed";

        private readonly FakeWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public UserAnimalsControllerTests(FakeWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateDefaultClient();
        }

        [Fact]
        public async Task GetAll_WhenUserFound_ShouldReturnUserAnimals()
        {
            var userId = 1;

            var response = await _client.GetAsync(string.Format(GetUserAnimalsEndpoint, userId));

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var animals = JsonConvert.DeserializeObject<Animal[]>(content, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
            Assert.NotNull(animals);
            Assert.Equal(2, animals.Length);
            Assert.Equal("dog", animals[1].Type);
        }

        [Fact]
        public async Task Get_WhenUserAndAnimalFound_ShouldReturnAnimal()
        {
            var userId = 1;
            var animalId = 2;

            var response = await _client.GetAsync(string.Format(GetUserAnimalEndpoint, userId, animalId));
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var animal = JsonConvert.DeserializeObject<Animal>(content);
            Assert.NotNull(animal);
            Assert.Equal("dog", animal.Type);
        }

        [Fact]
        public async Task Adopt_WhenUserAndAnimalFound_ShouldAdoptTheAnimal()
        {
            var key = nameof(User);
            var userId = 2;
            var code = 4;
            var adopt = new UserCommands.Adopt { Code = code };

            var response = await _client.PostAsJsonAsync(string.Format(AdoptAnimalEndpoint, userId), adopt);

            var users = _factory.FakeUsersCache.Get<List<User>>(key);
            var user = users.FirstOrDefault(u => u.Id == userId);
            Assert.Single(user.Animals);
            Assert.Contains(user.Animals, animal => animal.Code == 4);
        }

        [Fact]
        public async Task Stroke_WhenAnimalFound_ShouldIncreaseHappiness()
        {
            var key = nameof(User);
            var userId = 1;
            var animalId = 1;
            var users = _factory.FakeUsersCache.Get<List<User>>(key);
            var user = users.FirstOrDefault(u => u.Id == userId);
            var animal = user.Animals.FirstOrDefault(a => a.Id == animalId);
            var happinessBeforeStroke = animal.Metrics[MetricType.Happiness].Value;
            var stroke = new UserCommands.Stroke { AnimalId = animalId };

            var response = await _client.PostAsJsonAsync(string.Format(StrokeAnimalEndpoint, userId), stroke);

            var animalOptions = _factory.Services.GetService<IOptions<AnimalOptions>>();
            var animalStat = animalOptions.Value.AnimalStats.FirstOrDefault(s => s.Code == animal.Code);
            var currentHappiness = animal.Metrics[MetricType.Happiness].Value;
            Assert.Equal(animalStat.StrokeHappinessBoost, currentHappiness - happinessBeforeStroke);
        }

        [Fact]
        public async Task Feed_WhenAnimalFound_ShouldDecreaseHunger()
        {
            var key = nameof(User);
            var userId = 1;
            var animalId = 1;
            var users = _factory.FakeUsersCache.Get<List<User>>(key);
            var user = users.FirstOrDefault(u => u.Id == userId);
            var animal = user.Animals.FirstOrDefault(a => a.Id == animalId);
            var hungerBeforeFeed = animal.Metrics[MetricType.Hunger].Value;
            var feed = new UserCommands.Feed { AnimalId = animalId };

            var response = await _client.PostAsJsonAsync(string.Format(FeedAnimalEndpoint, userId), feed);

            var animalOptions = _factory.Services.GetService<IOptions<AnimalOptions>>();
            var animalStat = animalOptions.Value.AnimalStats.FirstOrDefault(s => s.Code == animal.Code);
            var currentHunger = animal.Metrics[MetricType.Hunger].Value;
            Assert.Equal(animalStat.FeedHungerBoost, currentHunger - hungerBeforeFeed);
        }

        public void Dispose()
        {
            _factory.ResetCache();
        }
    }
}
