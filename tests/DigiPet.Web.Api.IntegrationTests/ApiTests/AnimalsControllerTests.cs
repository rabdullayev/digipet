using DigiPet.Web.Api.Configs;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace DigiPet.Web.Api.IntegrationTests.ApiTests
{
    public class AnimalsControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string AnimalsEndpoint = "/api/animals";

        private readonly WebApplicationFactory<Startup> _factory;

        public AnimalsControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_Always_ShouldReturnAnimalStatsFromConfig()
        {
            var basePath = Directory.GetCurrentDirectory();
            var path = Path.Combine(basePath, "appsettings.json");
            var httpClient = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config
                    .SetBasePath(basePath)
                    .AddJsonFile(path);
                });
            }).CreateDefaultClient();

            var response = await httpClient.GetFromJsonAsync<AnimalStat[]>(AnimalsEndpoint);

            Assert.NotNull(response);
            Assert.Equal(4, response.Length);
            Assert.Equal("mice", response[0].Type);
            Assert.Equal(1, response[0].Code);
            Assert.Equal(3.0f, response[0].HappinessDecreaseRate);
            Assert.Equal(-3.0f, response[0].HungerDecreaseRate);
            Assert.Equal(3.0f, response[0].StrokeHappinessBoost);
            Assert.Equal(-3.0f, response[0].FeedHungerBoost);
        }
    }
}
