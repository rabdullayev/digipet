using System;
using System.Threading;
using System.Threading.Tasks;
using DigiPet.Web.Api.Application.Events;
using DigiPet.Web.Api.Configs;
using DigiPet.Web.Api.Infrastructure.Messaging.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DigiPet.Web.Api.Services
{
    /// <summary>
    /// Simple Game Server that publishes <see cref="GameServerEvents.TickEvent"/> on a constant interval
    /// </summary>
    public class HostedGameServer : IHostedService, IDisposable
    {
        private readonly IHub _hub;
        private readonly IOptions<GameOptions> _gameOptions;
        private readonly ILogger<HostedGameServer> _logger;

        private Timer _timer;
        private float _delta;

        public HostedGameServer(IHub hub, IOptions<GameOptions> gameOptions, ILogger<HostedGameServer> logger)
        {
            _hub = hub;
            _gameOptions = gameOptions;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting the game server...");

            _delta = _gameOptions.Value.GameClockRateMs / 1_000;
            _timer = new Timer(EmitTickEvent, null, TimeSpan.Zero, TimeSpan.FromSeconds(_delta));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping the game server...");

            _timer?.Change(Timeout.Infinite, 0);

            _logger.LogInformation("Stopping has stopped!");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void EmitTickEvent(object state)
        {
            _hub.Publish(new GameServerEvents.TickEvent { DeltaTime = _delta });
        }
    }
}
