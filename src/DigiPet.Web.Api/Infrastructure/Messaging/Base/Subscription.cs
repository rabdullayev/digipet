using System;
using DigiPet.Web.Api.Infrastructure.Messaging.Contracts;

namespace DigiPet.Web.Api.Infrastructure.Messaging.Base
{
    public class Subscription : ISubscription
    {
        public Subscription(Delegate @delegate)
        {
            Delegate = @delegate;
        }
        public Delegate Delegate { get; }

        #region Resharper Disposable Pattern

        private void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~Subscription()
        {
            ReleaseUnmanagedResources();
        }

        #endregion
    }
}