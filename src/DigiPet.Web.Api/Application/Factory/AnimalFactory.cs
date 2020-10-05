using System.Collections.Generic;
using System.Linq;
using DigiPet.Web.Api.Configs;
using DigiPet.Web.Api.Domain.Model;
using DigiPet.Web.Api.Domain.Model.Metrics;
using DigiPet.Web.Api.Infrastructure.Persistence.Contracts;
using Microsoft.Extensions.Options;

namespace DigiPet.Web.Api.Application.Factory
{
    /// <summary>
    /// A factory to create an instance of the animal.
    /// Animals don't own an instance before they get adopted.
    /// </summary>
    public interface IAnimalFactory
    {
        Animal Create(int code);
    }
    public class AnimalFactory : IAnimalFactory
    {
        private const float Neutral = .0f;
        private readonly IUniqueIdProvider _uniqueIdProvider;
        private readonly IOptions<AnimalOptions> _animalOptions;

        public AnimalFactory(IUniqueIdProvider uniqueIdProvider, IOptions<AnimalOptions> animalOptions)
        {
            _uniqueIdProvider = uniqueIdProvider;
            _animalOptions = animalOptions;
        }

        public Animal Create(int code)
        {
            var animalStat = _animalOptions.Value.AnimalStats.FirstOrDefault(a => a.Code == code);
            if (animalStat == null)
            {
                return null;
            }

            var metrics = new Dictionary<MetricType, Metric>
            {
                { MetricType.Happiness, new Happiness(Neutral, animalStat.HappinessDecreaseRate) },
                { MetricType.Hunger, new Hunger(Neutral,animalStat.HungerDecreaseRate) }
            };

            var id = _uniqueIdProvider.NextAnimal();
            var animal = new Animal(id, code, animalStat.Type, metrics);

            return animal;
        }
    }
}