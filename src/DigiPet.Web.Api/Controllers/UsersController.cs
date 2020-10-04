using System.Threading.Tasks;
using DigiPet.Web.Api.Application.Commands;
using DigiPet.Web.Api.Configs;
using DigiPet.Web.Api.Domain.Model;
using DigiPet.Web.Api.Infrastructure.Persistence.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DigiPet.Web.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUniqueIdProvider _idProvider;

        public UsersController(IUserRepository userRepository, IUniqueIdProvider idProvider)
        {
            _userRepository = userRepository;
            _idProvider = idProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userRepository.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{id:int}", Name = RouteNames.GetUser)]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _userRepository.GetByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCommands.Create createUser)
        {
            var user = new User(_idProvider.NextUser(), createUser.Username);

            await _userRepository.InsertAsync(user);

            return CreatedAtRoute(RouteNames.GetUser, new { id = user.Id }, user);
        }
    }
}
