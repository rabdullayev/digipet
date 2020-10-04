using DigiPet.Web.Api.Domain.Model.Metrics;
using Xunit;

namespace DigiPet.Web.Api.Tests.DomainTests
{
    public class MetricTests
    {
        [Fact]
        public void Decrease_Always_ShouldSubtractDecreaseRatexDeltaFromValue()
        {
            var hunger = new Hunger(3.0f, 2.0f);
            var happiness = new Happiness(0.0f, 3.0f);

            hunger.Decrease(3.0f);
            happiness.Decrease(2.0f);

            Assert.Equal(-3.0f, hunger.Value);
            Assert.Equal(-6.0f, happiness.Value);
        }

        [Fact]
        public void Boost_Always_ShouldAddAmountToValue()
        {
            var hunger = new Hunger(3.0f, 2.0f);
            var happiness = new Happiness(0.0f, 3.0f);

            hunger.Boost(3.0f);
            happiness.Boost(2.0f);

            Assert.Equal(6.0f, hunger.Value);
            Assert.Equal(2.0f, happiness.Value);
        }
    }
}
