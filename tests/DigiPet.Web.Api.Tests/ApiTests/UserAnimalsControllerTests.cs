using System.Collections.Generic;
using System.Threading.Tasks;
using DigiPet.Web.Api.Application.Commands;
using DigiPet.Web.Api.Application.Factory;
using DigiPet.Web.Api.Configs;
using DigiPet.Web.Api.Controllers;
using DigiPet.Web.Api.Domain.Model;
using DigiPet.Web.Api.Domain.Model.Metrics;
using DigiPet.Web.Api.Infrastructure.Messaging.Contracts;
using DigiPet.Web.Api.Infrastructure.Persistence.Contracts;
using DigiPet.Web.Api.Tests.TestHelpers;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Xunit;

namespace DigiPet.Web.Api.Tests.ApiTests
{
    public class UserAnimalsControllerTests
    {
        private readonly IOptions<AnimalOptions> _options;
        private readonly IUserRepository _userRepository;
        private readonly IAnimalFactory _animalFactory;
        private readonly IHub _hub;

        public UserAnimalsControllerTests()
        {
            _options = Options.Create<AnimalOptions>(new AnimalOptions
            {
                AnimalStats = new[] { new AnimalStat
                {
                    Code = 1,
                    Type = "dummy"
                } }
            });
            _userRepository = A.Fake<IUserRepository>();
            _animalFactory = A.Fake<IAnimalFactory>();
            _hub = A.Fake<IHub>();
        }

        [Fact]
        public async Task GetAll_WhenUserNotFound_ShouldReturnNotFound()
        {
            var controller = GetUserAnimalsController();
            A.CallTo(() => _userRepository.GetByIdAsync(A<int>._)).Returns(Task.FromResult<User>(null));

            var result = await controller.GetAll(1);

            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAll_WhenUserFound_ShouldReturnUserAnimals()
        {
            var animal = AnimalBuilder.NewInstance()
                .WithId(1)
                .WithCode(2)
                .Build();
            var user = UserBuilder.NewInstance()
                .WithId(1)
                .WithUsername("fakeuser")
                .AddAnimal(animal)
                .Build();
            var controller = GetUserAnimalsController();
            A.CallTo(() => _userRepository.GetByIdAsync(A<int>._)).Returns(Task.FromResult<User>(user));

            var result = await controller.GetAll(1);

            Assert.NotNull(result);
            var userAnimalsResult = Assert.IsType<OkObjectResult>(result);
            var animals = Assert.IsType<List<Animal>>(userAnimalsResult.Value);
            Assert.Single(animals);
            Assert.Same(animal, animals[0]);
        }

        [Fact]
        public async Task Get_WhenUserNotFound_ShouldReturnNotFound()
        {
            var controller = GetUserAnimalsController();
            A.CallTo(() => _userRepository.GetByIdAsync(A<int>._)).Returns(Task.FromResult<User>(null));

            var result = await controller.Get(1, 1);

            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Get_WhenAnimalNotFound_ShouldReturnNotFound()
        {
            var animal = AnimalBuilder.NewInstance()
                .WithId(1)
                .WithCode(2)
                .Build();
            var user = UserBuilder.NewInstance()
                .WithId(1)
                .WithUsername("fakeuser")
                .AddAnimal(animal)
                .Build();
            var controller = GetUserAnimalsController();
            A.CallTo(() => _userRepository.GetByIdAsync(A<int>._)).Returns(Task.FromResult<User>(user));

            var result = await controller.Get(1, 2);

            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Get_WhenAnimalFound_ShouldReturnTheAnimalResult()
        {
            var animal = AnimalBuilder.NewInstance()
                .WithId(1)
                .WithCode(2)
                .Build();
            var user = UserBuilder.NewInstance()
                .WithId(1)
                .WithUsername("fakeuser")
                .AddAnimal(animal)
                .Build();
            var controller = GetUserAnimalsController();
            A.CallTo(() => _userRepository.GetByIdAsync(A<int>._)).Returns(Task.FromResult<User>(user));

            var result = await controller.Get(1, 1);

            Assert.NotNull(result);
            var animalResult = Assert.IsType<OkObjectResult>(result);
            var animalResultValue = Assert.IsType<Animal>(animalResult.Value);
            Assert.Same(animalResultValue, animal);
        }

        [Fact]
        public async Task Adopt_WhenUserNotFound_ShouldReturnBadRequest()
        {
            var controller = GetUserAnimalsController();
            A.CallTo(() => _userRepository.GetByIdAsync(A<int>._)).Returns(Task.FromResult<User>(null));

            var result = await controller.Adopt(1, A.Dummy<UserCommands.Adopt>());

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Adopt_WhenAnimalNotFound_ShouldReturnBadRequest()
        {
            var user = UserBuilder.NewInstance()
                .WithId(1)
                .WithUsername("fakeuser")
                .Build();
            var controller = GetUserAnimalsController();
            A.CallTo(() => _userRepository.GetByIdAsync(A<int>._)).Returns(Task.FromResult<User>(user));
            A.CallTo(() => _animalFactory.Create(A<int>._)).Returns(null);

            var result = await controller.Adopt(1, new UserCommands.Adopt { Code = 2 });

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Adopt_WhenUserAndAnimalFound_ShouldReturnCreatedAtRoute()
        {
            var animal = AnimalBuilder.NewInstance()
                .WithCode(1)
                .Build();
            var user = UserBuilder.NewInstance()
                .WithId(1)
                .WithUsername("fakeuser")
                .Build();
            var controller = GetUserAnimalsController();
            A.CallTo(() => _userRepository.GetByIdAsync(A<int>._)).Returns(Task.FromResult<User>(user));
            A.CallTo(() => _animalFactory.Create(A<int>._)).Returns(animal);

            var result = await controller.Adopt(1, new UserCommands.Adopt { Code = 1 });

            Assert.NotNull(result);
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);
            var createdAnimal = Assert.IsType<Animal>(createdAtRouteResult.Value);
            Assert.Contains(createdAtRouteResult.RouteValues.Keys, key => key == "userId");
            Assert.Contains(createdAtRouteResult.RouteValues.Keys, key => key == "animalId");
            Assert.Equal(1, createdAtRouteResult.RouteValues["userId"]);
            Assert.Equal(1, createdAnimal.Code);
        }

        [Fact]
        public async Task Stroke_WhenUserNotFound_ShouldReturnBadRequest()
        {
            var controller = GetUserAnimalsController();
            A.CallTo(() => _userRepository.GetByIdAsync(A<int>._)).Returns(Task.FromResult<User>(null));

            var result = await controller.Stroke(1, A.Dummy<UserCommands.Stroke>());

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Stroke_WhenAnimalNotFound_ShouldReturnBadRequest()
        {
            var animal = AnimalBuilder.NewInstance()
                .WithId(1)
                .WithCode(1)
                .Build();
            var user = UserBuilder.NewInstance()
                .WithId(1)
                .WithUsername("fakeuser")
                .AddAnimal(animal)
                .Build();
            var controller = GetUserAnimalsController();
            A.CallTo(() => _userRepository.GetByIdAsync(A<int>._)).Returns(Task.FromResult<User>(user));

            var result = await controller.Stroke(1, new UserCommands.Stroke { AnimalId = 2 });

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Stroke_WhenAnimalStatNotFound_ShouldReturnInternalServerError()
        {
            var animal = AnimalBuilder.NewInstance()
                .WithId(1)
                .WithCode(2)
                .Build();
            var user = UserBuilder.NewInstance()
                .WithId(1)
                .WithUsername("fakeuser")
                .AddAnimal(animal)
                .Build();
            var controller = GetUserAnimalsController();
            A.CallTo(() => _userRepository.GetByIdAsync(A<int>._)).Returns(Task.FromResult<User>(user));

            var result = await controller.Stroke(1, new UserCommands.Stroke { AnimalId = 1 });

            Assert.NotNull(result);
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Stroke_WhenUserAndAnimalFound_ShouldReturnOkResult()
        {
            var animal = AnimalBuilder.NewInstance()
                .WithId(1)
                .WithCode(1)
                .AddMetric(new Happiness(0.0f, 1.0f))
                .Build();
            var user = UserBuilder.NewInstance()
                .WithId(1)
                .WithUsername("fakeuser")
                .AddAnimal(animal)
                .Build();
            var controller = GetUserAnimalsController();
            A.CallTo(() => _userRepository.GetByIdAsync(A<int>._)).Returns(Task.FromResult<User>(user));

            var result = await controller.Stroke(1, new UserCommands.Stroke { AnimalId = 1 });

            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Feed_WhenUserNotFound_ShouldReturnBadRequest()
        {
            var controller = GetUserAnimalsController();
            A.CallTo(() => _userRepository.GetByIdAsync(A<int>._)).Returns(Task.FromResult<User>(null));

            var result = await controller.Feed(1, A.Dummy<UserCommands.Feed>());

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Feed_WhenAnimalNotFound_ShouldReturnBadRequest()
        {
            var animal = AnimalBuilder.NewInstance()
                .WithId(1)
                .WithCode(1)
                .Build();
            var user = UserBuilder.NewInstance()
                .WithId(1)
                .WithUsername("fakeuser")
                .AddAnimal(animal)
                .Build();
            var controller = GetUserAnimalsController();
            A.CallTo(() => _userRepository.GetByIdAsync(A<int>._)).Returns(Task.FromResult<User>(user));

            var result = await controller.Feed(1, new UserCommands.Feed { AnimalId = 2 });

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Feed_WhenAnimalStatNotFound_ShouldReturnInternalServerError()
        {
            var animal = AnimalBuilder.NewInstance()
                .WithId(1)
                .WithCode(2)
                .Build();
            var user = UserBuilder.NewInstance()
                .WithId(1)
                .WithUsername("fakeuser")
                .AddAnimal(animal)
                .Build();
            var controller = GetUserAnimalsController();
            A.CallTo(() => _userRepository.GetByIdAsync(A<int>._)).Returns(Task.FromResult<User>(user));

            var result = await controller.Feed(1, new UserCommands.Feed { AnimalId = 1 });

            Assert.NotNull(result);
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Feed_WhenUserAndAnimalFound_ShouldReturnOkResult()
        {
            var animal = AnimalBuilder.NewInstance()
                .WithId(1)
                .WithCode(1)
                .AddMetric(new Hunger(0.0f, 1.0f))
                .Build();
            var user = UserBuilder.NewInstance()
                .WithId(1)
                .WithUsername("fakeuser")
                .AddAnimal(animal)
                .Build();
            var controller = GetUserAnimalsController();
            A.CallTo(() => _userRepository.GetByIdAsync(A<int>._)).Returns(Task.FromResult<User>(user));

            var result = await controller.Feed(1, new UserCommands.Feed { AnimalId = 1 });

            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        #region Helpers

        private UserAnimalsController GetUserAnimalsController()
        {
            return new UserAnimalsController(_userRepository, _animalFactory, _hub, _options);
        }

        #endregion
    }
}
