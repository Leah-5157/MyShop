using Entities;
using System.Threading.Tasks;

namespace Services
{
    public interface IUserService
    {
        Task<User> RegisterAsync(User user);
        Task<User> LoginAsync(string userName, string password);
        Task UpdateAsync(int id, User userToUpdate);
        int GetPasswordStrength(string password);
        string HashPassword(string password, string salt);
        Task<string?> GetSaltByUserName(string userName);
    }
}