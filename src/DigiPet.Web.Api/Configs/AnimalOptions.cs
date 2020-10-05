namespace DigiPet.Web.Api.Configs
{
    /// <summary>
    /// Animal config injected through appsettings.json
    /// </summary>
    public class AnimalOptions
    {
        public const string ConfigSection = nameof(AnimalOptions);
        public AnimalStat[] AnimalStats { get; set; }
    }

    /// <summary>
    /// Contains the code, type and metric deltas for the animals
    /// </summary>
    public class AnimalStat
    {
        public int Code { get; set; }
        public string Type { get; set; }
        public float HappinessDecreaseRate { get; set; }
        public float HungerDecreaseRate { get; set; }
        public float StrokeHappinessBoost { get; set; }
        public float FeedHungerBoost { get; set; }
    }
}
