using System.Collections.Generic;
using DigiPet.Web.Api.Domain.Model;

namespace DigiPet.Web.Api.Tests.TestHelpers
{
    public class UserBuilder
    {
        public static UserBuilder NewInstance() => new UserBuilder();

        private int _id;
        private string _username;
        private readonly List<Animal> _animals;

        public UserBuilder()
        {
            _animals = new List<Animal>();
        }

        public UserBuilder WithId(int id)
        {
            _id = id;

            return this;
        }

        public UserBuilder WithUsername(string username)
        {
            _username = username;

            return this;
        }

        public UserBuilder AddAnimal(Animal animal)
        {
            _animals.Add(animal);

            return this;
        }

        public User Build()
        {
            var user = new User(_id, _username);
            user.Animals.AddRange(_animals);

            return user;
        }
    }
}
