namespace DigiPet.Web.Api.Infrastructure.Persistence.Contracts
{
    public interface IUniqueIdProvider
    {
        int NextUser();
        int NextAnimal();
    }
}
