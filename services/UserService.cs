using Entities;
using Repositories;
using System.Security.Cryptography;
using System.Text;
using Zxcvbn;

namespace Services
{


    public class UserService : IUserService
    {
        IUserRepository _UserRepository;
        public UserService(IUserRepository userRepository) {
            _UserRepository = userRepository;
        }
      

        // GET api/<UsersController>/5

        public string Get(int id)
        {
            return "value";
        }

        // Registration: hash password and store salt
        public async Task<User> Post(User user)
        {
            // Generate salt
            user.Salt = GenerateSalt();
            // Hash password with salt
            user.Password = HashPassword(user.Password, user.Salt);
            return await _UserRepository.Post(user);
        }

        // Login: verify password
        public async Task<User> Login(string UserName, string Password)
        {
            // Step 1: Get salt for username
            var salt = await _UserRepository.GetSaltByUserName(UserName);
            if (salt == null)
                return null;
            // Step 2: Hash entered password with salt
            var hashed = HashPassword(Password, salt);
            // Step 3: Query for user with username and hashed password
            return await _UserRepository.Login(UserName, hashed);
        }

        // PUT api/<UsersController>/5

        public async Task Put(int id, User userToUpdate)
        {
            // If password is being updated, hash it
            if (!string.IsNullOrEmpty(userToUpdate.Password))
            {
                userToUpdate.Salt = GenerateSalt();
                userToUpdate.Password = HashPassword(userToUpdate.Password, userToUpdate.Salt);
            }
            await _UserRepository.Put(id, userToUpdate);
        }
        public int  Password(string Password)
        {
            var result = Zxcvbn.Core.EvaluatePassword(Password);
            int Result = result.Score;
            return Result;
        }


        // Helper: Generate random salt
        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        // Helper: Hash password with salt using PBKDF2
        private string HashPassword(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256))
            {
                return Convert.ToBase64String(pbkdf2.GetBytes(32));
            }
        }
    }
}
