using System.Threading;
using DigiPet.Web.Api.Infrastructure.Persistence.Contracts;

namespace DigiPet.Web.Api.Infrastructure.Persistence.InMemory
{
    public class UniqueIdProvider : IUniqueIdProvider
    {
        private static int _userId;
        private static int _animalId;

        public int NextUser()
        {
            return Interlocked.Increment(ref _userId);
        }

        public int NextAnimal()
        {
            return Interlocked.Increment(ref _animalId);
        }
    }
}
