using System.Linq;
using System.Threading.Tasks;
using DigiPet.Web.Api.Application.Commands;
using DigiPet.Web.Api.Application.Events;
using DigiPet.Web.Api.Application.Factory;
using DigiPet.Web.Api.Configs;
using DigiPet.Web.Api.Infrastructure.Messaging.Contracts;
using DigiPet.Web.Api.Infrastructure.Persistence.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DigiPet.Web.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserAnimalsController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAnimalFactory _animalFactory;
        private readonly IHub _hub;
        private readonly IOptions<AnimalOptions> _animalOptions;

        public UserAnimalsController(IUserRepository userRepository, IAnimalFactory animalFactory, IHub hub, IOptions<AnimalOptions> animalOptions)
        {
            _userRepository = userRepository;
            _animalFactory = animalFactory;
            _hub = hub;
            _animalOptions = animalOptions;
        }

        [HttpGet("{userId:int}/animals")]
        public async Task<IActionResult> GetAll(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return NotFound();

            return Ok(user.Animals);
        }

        [HttpGet("{userId:int}/animals/{animalId:int}", Name = RouteNames.GetUserAnimal)]
        public async Task<IActionResult> Get(int userId, int animalId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return NotFound();

            var animal = user.Animals.FirstOrDefault(a => a.Id == animalId);

            if (animal == null)
                return NotFound();

            return Ok(animal);
        }

        [HttpPost("{userId:int}/animals/adopt")]
        public async Task<IActionResult> Adopt(int userId, [FromBody] UserCommands.Adopt adopt)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return BadRequest($"User with id:{userId} doesn't exist.");
            }

            var animal = _animalFactory.Create(adopt.Code);
            if (animal == null)
            {
                return BadRequest($"{adopt.Code} doesn't correspond to any animal.");
            }

            await _userRepository.Adopt(user, animal);

            _hub.Publish(new UserEvents.AnimalAdopted { Animal = animal });

            return CreatedAtRoute(RouteNames.GetUserAnimal, new { userId = user.Id, animalId = animal.Id }, animal);
        }

        [HttpPost("{userId:int}/animals/stroke")]
        public async Task<IActionResult> Stroke(int userId, [FromBody] UserCommands.Stroke stroke)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return BadRequest($"User with id:{userId} doesn't exist.");
            }

            var animal = user.Animals.FirstOrDefault(a => a.Id == stroke.AnimalId);
            if (animal == null)
            {
                return BadRequest($"User with id:{userId} doesn't own an animal with id:{stroke.AnimalId}.");
            }

            var animalStats = _animalOptions.Value.AnimalStats.FirstOrDefault(s => s.Code == animal.Code);
            if (animalStats == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong, please contact the administrator.");
            }

            animal.Stroke(animalStats.StrokeHappinessBoost);

            return Ok();
        }

        [HttpPost("{userId:int}/animals/feed")]
        public async Task<IActionResult> Feed(int userId, [FromBody] UserCommands.Feed feed)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return BadRequest($"User with id:{userId} doesn't exist.");
            }

            var animal = user.Animals.FirstOrDefault(a => a.Id == feed.AnimalId);
            if (animal == null)
            {
                return BadRequest($"User with id:{userId} doesn't own an animal with id:{feed.AnimalId}.");
            }

            var animalStats = _animalOptions.Value.AnimalStats.FirstOrDefault(s => s.Code == animal.Code);
            if (animalStats == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong, please contact the administrator.");
            }

            animal.Feed(animalStats.FeedHungerBoost);

            return Ok();
        }
    }
}
