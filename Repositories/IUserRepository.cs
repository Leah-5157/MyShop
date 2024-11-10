using Entities;

namespace Repositories
{
    public interface IUserRepository
    {
        void Delete(int id);
        IEnumerable<string> Get();
        string Get(int id);
        User Login(string UserName, string Password);
        User Post(User user);
        User Put(int id, User userToUpdate);
    }
}