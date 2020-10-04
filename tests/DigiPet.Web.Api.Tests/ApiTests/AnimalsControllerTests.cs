using DigiPet.Web.Api.Configs;
using DigiPet.Web.Api.Controllers;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Xunit;

namespace DigiPet.Web.Api.Tests.ApiTests
{
    public class AnimalsControllerTests
    {
        [Fact]
        public void Get_Always_ReturnsAnimalStatsFromConfig()
        {
            var animalStat = A.Dummy<AnimalStat>();
            var animalStats = new[] { animalStat };
            var options = Options.Create<AnimalOptions>(new AnimalOptions { AnimalStats = animalStats });
            var controller = new AnimalsController(options);

            var result = controller.Get();

            Assert.NotNull(result);
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var statsResult = Assert.IsType<AnimalStat[]>(objectResult.Value);
            Assert.Single(statsResult);
            Assert.Same(animalStat, statsResult[0]);
        }
    }
}
