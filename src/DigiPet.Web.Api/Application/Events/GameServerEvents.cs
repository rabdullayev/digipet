using DigiPet.Web.Api.Infrastructure.Messaging.Base;

namespace DigiPet.Web.Api.Application.Events
{
    /// <summary>
    /// Container for Game Server Events
    /// </summary>
    public class GameServerEvents
    {
        /// <summary>
        /// An event sent from the <see cref="Services.HostedGameServer"/> on a regular basis
        /// The interval of the event can be adjusted in GameOptions section in appsettings.json
        /// </summary>
        public class TickEvent : Event
        {
            public float DeltaTime { get; set; }
        }
    }
}
