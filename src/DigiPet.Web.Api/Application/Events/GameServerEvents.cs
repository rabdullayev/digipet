using DigiPet.Web.Api.Infrastructure.Messaging.Base;

namespace DigiPet.Web.Api.Application.Events
{
    public class GameServerEvents
    {
        public class TickEvent : Event
        {
            public float DeltaTime { get; set; }
        }
    }
}
