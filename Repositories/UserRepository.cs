using System.Text.Json;
using Entities;
namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        public UserRepository() { }
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
            using (StreamReader reader = System.IO.File.OpenText("C:\\Users\\sarahf\\source\\repos\\MyShopDisin\\MyShop\\UserList.txt"))
            {
                string? currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {
                    User user = JsonSerializer.Deserialize<User>(currentUserInFile);
                    if (user.UserName == UserName && user.Password == Password)
                        return user;
                }
            }
            return null;
        }

        // POST api/<UsersController>


        public User Post(User user)
        {
            int numberOfUsers = System.IO.File.ReadLines("C:\\Users\\sarahf\\source\\repos\\MyShopDisin\\MyShop\\UserList.txt").Count();
            user.userId = numberOfUsers + 1;
            string userJson = JsonSerializer.Serialize(user);
            System.IO.File.AppendAllText("C:\\Users\\sarahf\\source\\repos\\MyShopDisin\\MyShop\\UserList.txt", userJson + Environment.NewLine);
            return (user);

        }

        // PUT api/<UsersController>/5

        public User Put(int id, User userToUpdate)
        {
            string textToReplace = string.Empty;
            userToUpdate.userId = id;
            using (StreamReader reader = System.IO.File.OpenText("C:\\Users\\sarahf\\source\\repos\\MyShopDisin\\MyShop\\UserList.txt"))
            {
                string currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {

                    User user = JsonSerializer.Deserialize<User>(currentUserInFile);
                    if (user.userId == id)
                        textToReplace = currentUserInFile;
                }
            }

            if (textToReplace != string.Empty)
            {
                string text = System.IO.File.ReadAllText("C:\\Users\\sarahf\\source\\repos\\MyShopDisin\\MyShop\\UserList.txt");
                text = text.Replace(textToReplace, JsonSerializer.Serialize(userToUpdate));
                System.IO.File.WriteAllText("C:\\Users\\sarahf\\source\\repos\\MyShopDisin\\MyShop\\UserList.txt", text);
                return userToUpdate;
            }

            return null;
        }

        // DELETE api/<UsersController>/5

        public void Delete(int id)
        {
        }
    }

}
