using System;
using DigiPet.Web.Api.Infrastructure.Messaging.Contracts;

namespace DigiPet.Web.Api.Infrastructure.Messaging.Base
{
    /// <summary>
    /// Base class for domain events
    /// </summary>
    public class Event : IEvent
    {
        public Event()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; }
    }
}
