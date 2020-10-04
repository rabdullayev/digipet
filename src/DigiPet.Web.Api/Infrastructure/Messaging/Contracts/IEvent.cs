using System;

namespace DigiPet.Web.Api.Infrastructure.Messaging.Contracts
{
    public interface IEvent
    {
        Guid Id { get; }
    }
}
