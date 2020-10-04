using System;
using DigiPet.Web.Api.Infrastructure.Messaging.Contracts;

namespace DigiPet.Web.Api.Infrastructure.Messaging.Base
{
    public class Event : IEvent
    {
        public Event()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; }
    }
}
