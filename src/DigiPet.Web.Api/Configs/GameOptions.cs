namespace DigiPet.Web.Api.Configs
{
    /// <summary>
    /// Game server config, injected through appsettings.json
    /// GameClockRateMs can be configured to adjust the rate of <see cref="Application.Events.GameServerEvents.TickEvent"/>
    /// </summary>
    public class GameOptions
    {
        public const string ConfigSection = nameof(GameOptions);

        public float GameClockRateMs { get; set; }
    }
}
