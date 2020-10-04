namespace DigiPet.Web.Api.Domain.Model.Metrics
{
    public class Happiness : Metric
    {
        public override MetricType Type => MetricType.Happiness;

        public Happiness(float value, float decreaseRate) : base(value, decreaseRate)
        {
        }
    }
}