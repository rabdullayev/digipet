using DigiPet.Web.Api.Domain.Model;
using DigiPet.Web.Api.Infrastructure.Messaging.Base;

namespace DigiPet.Web.Api.Application.Events
{
    public class UserEvents
    {
        public class AnimalAdopted : Event
        {
            public Animal Animal { get; set; }
        }
    }
}
