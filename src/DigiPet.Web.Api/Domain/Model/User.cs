using System.Collections.Generic;

namespace DigiPet.Web.Api.Domain.Model
{
    public class User : EntityBase
    {
        public User(int id, string username) : base(id)
        {
            Username = username;
            Animals = new List<Animal>();
        }

        public string Username { get; }
        public List<Animal> Animals { get; }
    }
}
