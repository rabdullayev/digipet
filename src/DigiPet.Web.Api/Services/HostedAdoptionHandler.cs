using System.Threading;
using System.Threading.Tasks;
using DigiPet.Web.Api.Application.Events;
using DigiPet.Web.Api.Infrastructure.Messaging.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DigiPet.Web.Api.Services
{
    /// <summary>
    /// After an animal gets adopted, it subscribes to <see cref="GameServerEvents.TickEvent"/>
    /// </summary>
    public class HostedAdoptionHandler : IHostedService
    {
        private readonly IHub _hub;
        private readonly ILogger<HostedAdoptionHandler> _logger;
        
        public HostedAdoptionHandler(ILogger<HostedAdoptionHandler> logger, IHub hub)
        {
            _logger = logger;
            _hub = hub;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _hub.Subscribe<UserEvents.AnimalAdopted>(e =>
            {
                _logger.LogInformation($"Adopted animal {e.Animal.Id} is subscribing to tick event.");
                _hub.Subscribe<GameServerEvents.TickEvent>(tick => e.Animal.Handle(tick));
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
