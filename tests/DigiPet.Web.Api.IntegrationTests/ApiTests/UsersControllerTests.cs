using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DigiPet.Web.Api.Application.Commands;
using DigiPet.Web.Api.Domain.Model;
using DigiPet.Web.Api.IntegrationTests.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Xunit;

namespace DigiPet.Web.Api.IntegrationTests.ApiTests
{
    public class UsersControllerTests : IClassFixture<FakeWebApplicationFactory<Startup>>, IDisposable
    {
        private const string GetUsersEndpoint = "/api/users";
        private const string GetUserEndpoint = "/api/users/{0}";
        private const string CreateUserEndpoint = "/api/users";

        private readonly FakeWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;
        public UsersControllerTests(FakeWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateDefaultClient();
        }

        [Fact]
        public async Task GetAll_Always_ShouldReturnUsersFromDatabase()
        {
            //Unfortunately, deserialization of reference types without parameterless constructor is not supported in System.Text.Json.JsonSerializer
            //var users = await _client.GetFromJsonAsync<User[]>(GetUsersEndpoint);
            //have to use Newtonsoft.Json here
            var response = await _client.GetAsync(GetUsersEndpoint);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<User[]>(content, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

            Assert.NotNull(users);
            Assert.Equal(3, users.Length);
            Assert.Equal("Wick", users[1].Username);
        }

        [Fact]
        public async Task Get_WhenUserExists_ShouldReturnUserFromDatabase()
        {
            var response = await _client.GetAsync(string.Format(GetUserEndpoint, 3));
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<User>(content);

            Assert.NotNull(users);
            Assert.Equal("Ben", users.Username);
        }

        [Fact]
        public async Task Create_Always_ShouldCreateUser()
        {
            var key = nameof(User);
            var create = new UserCommands.Create { Username = "fakeusername" };

            var response = await _client.PostAsJsonAsync(CreateUserEndpoint, create);

            var users = _factory.FakeUsersCache.Get<List<User>>(key);
            Assert.Contains(users, user => user.Username.Equals("fakeusername"));
        }

        public void Dispose()
        {
            _factory?.ResetCache();
        }
    }
}
