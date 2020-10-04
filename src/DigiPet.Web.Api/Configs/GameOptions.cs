namespace DigiPet.Web.Api.Configs
{
    public class GameOptions
    {
        public const string ConfigSection = nameof(GameOptions);

        public float GameClockRateMs { get; set; }
    }
}
