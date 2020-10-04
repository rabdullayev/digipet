using System;

namespace DigiPet.Web.Api.Infrastructure.Messaging.Contracts
{
    public interface ISubscription : IDisposable
    {
        Delegate Delegate { get; }
    }
}