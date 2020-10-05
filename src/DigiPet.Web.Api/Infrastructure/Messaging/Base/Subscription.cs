using System;
using DigiPet.Web.Api.Infrastructure.Messaging.Contracts;

namespace DigiPet.Web.Api.Infrastructure.Messaging.Base
{
    /// <summary>
    /// Wrapper around the subscribed delegates
    /// </summary>
    public class Subscription : ISubscription
    {
        public Subscription(Delegate @delegate)
        {
            Delegate = @delegate;
        }
        
        public Delegate Delegate { get; }
        
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}