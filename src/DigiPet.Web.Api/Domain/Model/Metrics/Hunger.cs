namespace DigiPet.Web.Api.Domain.Model.Metrics
{
    public class Hunger : Metric
    {
        public override MetricType Type => MetricType.Hunger;

        public Hunger(float value, float decreaseRate) : base(value, decreaseRate)
        {
        }
    }
}