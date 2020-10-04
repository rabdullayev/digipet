using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiPet.Web.Api.Application.Commands;
using DigiPet.Web.Api.Controllers;
using DigiPet.Web.Api.Domain.Model;
using DigiPet.Web.Api.Infrastructure.Persistence.Contracts;
using DigiPet.Web.Api.Tests.TestHelpers;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace DigiPet.Web.Api.Tests.ApiTests
{
    public class UsersControllerTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IUniqueIdProvider _uniqueIdProvider;

        public UsersControllerTests()
        {
            _userRepository = A.Fake<IUserRepository>();
            _uniqueIdProvider = A.Fake<IUniqueIdProvider>();
        }

        [Fact]
        public async Task GetAll_Always_ShouldReturnUsersFromRepository()
        {
            var user1 = UserBuilder.NewInstance()
                .WithId(1)
                .WithUsername("fakeuser1")
                .Build();
            var user2 = UserBuilder.NewInstance()
                .WithId(2)
                .WithUsername("fakeuser2")
                .Build();
            var users = new List<User> { user1, user2 };
            var controller = GetUsersController();
            A.CallTo(() => _userRepository.GetAllAsync()).Returns(Task.FromResult(users.AsEnumerable()));

            var result = await controller.GetAll();

            Assert.NotNull(result);
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var usersResult = Assert.IsType<List<User>>(objectResult.Value);
            Assert.Equal(2, usersResult.Count);
            Assert.Collection(usersResult,
                user => Assert.Same(user1, user),
                user => Assert.Same(user2, user));
        }

        [Fact]
        public async Task Get_WhenTheUserFound_ShouldReturnTheUser()
        {
            var user1 = UserBuilder.NewInstance()
                .WithId(1)
                .WithUsername("fakeuser1")
                .Build();
            var controller = GetUsersController();
            A.CallTo(() => _userRepository.GetByIdAsync(A<int>._)).Returns(Task.FromResult<User>(user1));

            var result = await controller.Get(1);

            Assert.NotNull(result);
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var userResult = Assert.IsType<User>(objectResult.Value);
            Assert.Same(user1, userResult);
        }

        [Fact]
        public async Task Get_WhenTheUserNotFound_ShouldReturnNotFound()
        {
            var controller = GetUsersController();
            A.CallTo(() => _userRepository.GetByIdAsync(A<int>._)).Returns(Task.FromResult<User>(null));

            var result = await controller.Get(1);

            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_Always_ShouldReturnCreatedAtRoute()
        {
            var createUser = new UserCommands.Create();
            var nextId = 1;
            var controller = GetUsersController();
            A.CallTo(() => _uniqueIdProvider.NextUser()).Returns(nextId);

            var result = await controller.Create(createUser);

            Assert.NotNull(result);
            var createdAtRoute = Assert.IsType<CreatedAtRouteResult>(result);
            var user = Assert.IsType<User>(createdAtRoute.Value);
            Assert.Equal(nextId, user.Id);
        }

        #region Helpers

        private UsersController GetUsersController()
        {
            return new UsersController(_userRepository, _uniqueIdProvider);
        }

        #endregion
    }
}
