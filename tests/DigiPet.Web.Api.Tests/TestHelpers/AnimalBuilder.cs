using System.Collections.Generic;
using DigiPet.Web.Api.Domain.Model;

namespace DigiPet.Web.Api.Tests.TestHelpers
{
    public class AnimalBuilder
    {
        public static AnimalBuilder NewInstance() => new AnimalBuilder();

        private int _id;
        private int _code;
        private readonly string _type = string.Empty;
        private readonly Dictionary<MetricType, Metric> _metrics;

        public AnimalBuilder()
        {
            _metrics = new Dictionary<MetricType, Metric>();
        }
        public AnimalBuilder WithId(int id)
        {
            _id = id;

            return this;
        }

        public AnimalBuilder WithCode(int code)
        {
            _code = code;

            return this;
        }

        public AnimalBuilder AddMetric(Metric metric)
        {
            _metrics.Add(metric.Type, metric);

            return this;
        }

        public Animal Build()
        {
            var animal = new Animal(_id, _code, _type, _metrics);

            return animal;
        }
    }
}
