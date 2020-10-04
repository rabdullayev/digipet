using System;

namespace DigiPet.Web.Api.Infrastructure.Messaging.Contracts
{
    public interface IHub
    {
        void Publish<TEvent>(TEvent @event) where TEvent : IEvent;

        ISubscription Subscribe<TEvent>(Action<TEvent> subscribedAction) where TEvent : IEvent;
    }
}