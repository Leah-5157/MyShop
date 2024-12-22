using Entities;

namespace Services
{
    public interface IUserService
    {
     
        string Get(int id);
        Task<User> Login(string UserName, string Password);
        Task<User> Post(User user);
        Task Put(int id, User userToUpdate);
        int Password(string password);
    }
}