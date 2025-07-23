using Entities;
using Repositories;
using System.Security.Cryptography;
using Zxcvbn;

namespace Services
{
    public static class PasswordHelper
    {
        public static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        public static string HashPassword(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256))
            {
                return Convert.ToBase64String(pbkdf2.GetBytes(32));
            }
        }
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) {
            _userRepository = userRepository;
        }

        public async Task<User> RegisterAsync(User user)
        {
            user.Salt = PasswordHelper.GenerateSalt();
            user.Password = PasswordHelper.HashPassword(user.Password, user.Salt);
            return await _userRepository.Post(user);
        }

        public async Task<User> LoginAsync(string userName, string password)
        {
            var salt = await _userRepository.GetSaltByUserName(userName);
            if (salt == null)
                return null;
            var hashed = PasswordHelper.HashPassword(password, salt);
            return await _userRepository.Login(userName, hashed);
        }

        public async Task UpdateAsync(int id, User userToUpdate)
        {
            // אין לבצע hashing או יצירת salt כאן - הכל כבר בוצע ב-Controller
            await _userRepository.Put(id, userToUpdate);
        }

        public int GetPasswordStrength(string password)
        {
            var result = Zxcvbn.Core.EvaluatePassword(password);
            return result.Score;
        }

        public string HashPassword(string password, string salt) => PasswordHelper.HashPassword(password, salt);
        public async Task<string?> GetSaltByUserName(string userName) => await _userRepository.GetSaltByUserName(userName);
    }
}
