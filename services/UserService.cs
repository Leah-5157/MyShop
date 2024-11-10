using Entities;
using Repositories;
using System.Text.Json;

namespace Services
{
    public class UserService
    {
        UserRepository repository = new();
        public UserService() { }
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
            return repository.Login(UserName, Password);
        }

        // POST api/<UsersController>


        public User Post(User user)
        {

            return repository.Post(user);

        }

        // PUT api/<UsersController>/5

        public User Put(int id, User userToUpdate)
        {
            return repository.Put(id, userToUpdate);
        }

        // DELETE api/<UsersController>/5

        public void Delete(int id)
        {
        }
    }
}
