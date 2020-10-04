namespace DigiPet.Web.Api.Configs
{
    public class AnimalOptions
    {
        public const string ConfigSection = nameof(AnimalOptions);
        public AnimalStat[] AnimalStats { get; set; }
    }

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
