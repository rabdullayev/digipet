using System.Collections.Generic;
using DigiPet.Web.Api.Application.Events;

namespace DigiPet.Web.Api.Domain.Model
{
    public class Animal : EntityBase
    {
        public Animal(int id, int code, string type, Dictionary<MetricType, Metric> metrics) : base(id)
        {
            Code = code;
            Type = type;
            Metrics = metrics;
        }
        public int Code { get; }
        public string Type { get; }
        public Dictionary<MetricType, Metric> Metrics { get; }
        
        public void Handle(GameServerEvents.TickEvent @event)
        {
            foreach (var metric in Metrics)
            {
                metric.Value.Decrease(@event.DeltaTime);
            }
        }

        public void Stroke(float amount)
        {
            Boost(MetricType.Happiness, amount);
        }

        public void Feed(float amount)
        {
            Boost(MetricType.Hunger, amount);
        }

        private void Boost(MetricType metricType, float amount)
        {
            Metrics[metricType].Boost(amount);
        }
    }
}