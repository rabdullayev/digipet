using DigiPet.Web.Api.Configs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DigiPet.Web.Api.Controllers
{
    [ApiController]
    [Route("api/animals")]
    public class AnimalsController : ControllerBase
    {
        private readonly IOptions<AnimalOptions> _animalOptions;

        public AnimalsController(IOptions<AnimalOptions> animalOptions)
        {
            _animalOptions = animalOptions;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_animalOptions.Value.AnimalStats);
        }
    }
}
