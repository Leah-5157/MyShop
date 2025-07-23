using Entities;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IUserRepository
    {
        Task<string?> GetSaltByUserName(string userName);
        Task<User> Login(string UserName, string Password);
        Task<User> Post(User user);
        Task Put(int id, User userToUpdate);
    }
}