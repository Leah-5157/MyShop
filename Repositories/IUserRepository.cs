using Entities;

namespace Repositories
{
    public interface IUserRepository
    {
        void Delete(int id);
        IEnumerable<string> Get();
        string Get(int id);
        Task<User> Login(string UserName, string Password);
        Task<User> Post(User user);
        Task Put(int id, User userToUpdate);
    }
}