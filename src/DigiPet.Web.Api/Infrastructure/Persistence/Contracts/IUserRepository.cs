using System.Collections.Generic;
using System.Threading.Tasks;
using DigiPet.Web.Api.Domain.Model;

namespace DigiPet.Web.Api.Infrastructure.Persistence.Contracts
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task InsertAsync(User user);
        Task Adopt(User user, Animal animal);
    }
}
