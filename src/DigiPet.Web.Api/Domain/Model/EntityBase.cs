namespace DigiPet.Web.Api.Domain.Model
{
    public abstract class EntityBase
    {
        protected EntityBase(int id)
        {
            Id = id;
        }
        public int Id { get; }
    }
}