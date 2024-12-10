﻿using System.Text.Json;
using Entities;
using Microsoft.EntityFrameworkCore;
namespace Repositories

{
    public class UserRepository : IUserRepository
    {
        MyShopContext _myShopContext;
        public UserRepository(MyShopContext myShopContext) {
            _myShopContext = myShopContext;
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

        public async Task<User> Login(string UserName, string Password)
        {
           return await _myShopContext.Users.FirstOrDefaultAsync(user=>user.Password==Password);
        }

        // POST api/<UsersController>


        public async Task<User> Post(User user)
        {
            //var res= _myShopContext.Users.Add(user);
            _myShopContext.Users.Add(user);
            await _myShopContext.SaveChangesAsync();
            //return res- the created user with the id
            return user;

        }

        // PUT api/<UsersController>/5

        public async Task Put(int id, User userToUpdate)//return user
        {
            userToUpdate.Id = id;
            _myShopContext.Users.Update(userToUpdate);
            await _myShopContext.SaveChangesAsync();
        }

        // DELETE api/<UsersController>/5

        public void Delete(int id)
        {
        }
    }

}
