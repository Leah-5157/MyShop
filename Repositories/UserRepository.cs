using System.Text.Json;
using Entities;
using Microsoft.EntityFrameworkCore;
using repositories;
namespace Repositories

{
    public class UserRepository : IUserRepository
    {
        MyShopContext _myShopContext;
        public UserRepository(MyShopContext myShopContext) {
            _myShopContext = myShopContext;
        }
        // GET api/<UsersController>/5

        public string Get(int id)
        {
            return "value";
        }

        public async Task<User> Login(string UserName, string Password)
        {
           return await _myShopContext.Users.FirstOrDefaultAsync(user=>user.Password==Password);
        }

        // POST api/<UsersController>


        public async Task<User> Post(User user)
        {
            var item = _myShopContext.Users.ToList().Find((item => item.UserName.Trim() == user.UserName));
            if (item == null)
            {
                _myShopContext.Users.Add(user);
                await _myShopContext.SaveChangesAsync();
                return user;
            }
            return null;
        }

        // PUT api/<UsersController>/5

        public async Task Put(int id, User userToUpdate)
        {
            userToUpdate.Id = id;
            _myShopContext.Users.Update(userToUpdate);
            await _myShopContext.SaveChangesAsync();
        }


    }

}
