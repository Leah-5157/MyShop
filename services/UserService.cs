using Entities;
using Repositories;
using System.Text.Json;
using Zxcvbn;

namespace Services
{


    public class UserService : IUserService
    {
        IUserRepository _UserRepository;
        public UserService(IUserRepository userRepository) {
            _UserRepository = userRepository;
        }
        // GET: api/<UsersController>

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UsersController>/5

        public string Get(int id)
        {
            return "value";
        }

        public User Login(string UserName, string Password)
        {
            return _UserRepository.Login(UserName, Password);
        }

        // POST api/<UsersController>


        public User Post(User user)
        {

            return _UserRepository.Post(user);// check password strength before sending to repository

        }

        // PUT api/<UsersController>/5

        public User Put(int id, User userToUpdate)
        {
            return _UserRepository.Put(id, userToUpdate);// check password strength before sending to repository
        }
        public int  Password(string Password)
        {
            var result = Zxcvbn.Core.EvaluatePassword(Password);
            int Result = result.Score;
            return Result;
        }

        // DELETE api/<UsersController>/5

        public void Delete(int id)
        {
        }
    }
}
