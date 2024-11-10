using Entities;

namespace Services
{
    public interface IUserService
    {
        void Delete(int id);
        IEnumerable<string> Get();
        string Get(int id);
        User Login(string UserName, string Password);
        User Post(User user);
        User Put(int id, User userToUpdate);
        int Password(string password);
    }
}