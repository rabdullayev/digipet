using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Xunit;

namespace DigiPet.Web.Api.IntegrationTests.ApiTests
{
    public class HealthCheckTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string HealthCheckEndpoint = "/api/health";

        private readonly WebApplicationFactory<Startup> _factory;
        public HealthCheckTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task HealthCheck_Always_ShouldReturnOk()
        {
            var httpClient = _factory.CreateDefaultClient();
            
            var response = await httpClient.GetAsync(HealthCheckEndpoint);

            response.EnsureSuccessStatusCode();
        }
    }
}
