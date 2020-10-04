using DigiPet.Web.Api.Application.Events;
using DigiPet.Web.Api.Domain.Model;
using DigiPet.Web.Api.Domain.Model.Metrics;
using DigiPet.Web.Api.Tests.TestHelpers;
using Xunit;

namespace DigiPet.Web.Api.Tests.DomainTests
{
    public class AnimalTests
    {
        [Fact]
        public void Stroke_Always_ShouldAddToHappiness()
        {
            var animal = AnimalBuilder.NewInstance()
                .AddMetric(new Happiness(0.0f, 1.0f))
                .Build();

            animal.Stroke(1.0f);

            Assert.Equal(1.0f, animal.Metrics[MetricType.Happiness].Value);

            animal.Stroke(-2.0f);

            Assert.Equal(-1.0f, animal.Metrics[MetricType.Happiness].Value);
        }

        [Fact]
        public void Feed_Always_ShouldAddToHunger()
        {
            var animal = AnimalBuilder.NewInstance()
                .AddMetric(new Hunger(0.0f, 1.0f))
                .Build();

            animal.Feed(1.0f);

            Assert.Equal(1.0f, animal.Metrics[MetricType.Hunger].Value);

            animal.Feed(-2.0f);

            Assert.Equal(-1.0f, animal.Metrics[MetricType.Hunger].Value);
        }

        [Fact]
        public void Handle_Always_ShouldDecreaseMetrics()
        {
            var serverDelta = 2.0f;
            var animal = AnimalBuilder.NewInstance()
                .AddMetric(new Happiness(0.0f, 1.0f))
                .AddMetric(new Hunger(0.0f, -1.0f))
                .Build();
            var tickEvent = new GameServerEvents.TickEvent { DeltaTime = serverDelta };

            animal.Handle(tickEvent);

            Assert.Equal(-2.0f, animal.Metrics[MetricType.Happiness].Value);
            Assert.Equal(2.0f, animal.Metrics[MetricType.Hunger].Value);

            animal.Handle(tickEvent);

            Assert.Equal(-4.0f, animal.Metrics[MetricType.Happiness].Value);
            Assert.Equal(4.0f, animal.Metrics[MetricType.Hunger].Value);
        }
    }
}
