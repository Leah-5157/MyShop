using Entities;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IUserRepository
    {
        string Get(int id);
        Task<string?> GetSaltByUserName(string userName);
        Task<User> Login(string UserName, string Password);
        Task<User> Post(User user);
        Task Put(int id, User userToUpdate);
    }
}