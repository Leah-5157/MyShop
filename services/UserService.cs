﻿using Entities;
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
      

        // GET api/<UsersController>/5

        public string Get(int id)
        {
            return "value";
        }

        public async Task<User> Login(string UserName, string Password)
        {
            return await _UserRepository.Login(UserName, Password);
        }

        // POST api/<UsersController>


        public async Task< User> Post(User user)
        {

            return await _UserRepository.Post(user);

        }

        // PUT api/<UsersController>/5

        public async Task Put(int id, User userToUpdate)
        {
           await _UserRepository.Put(id, userToUpdate);
        }
        public int  Password(string Password)
        {
            //you can write in in 1 line- return Zxcvbn.Core.EvaluatePassword(Password).Score
            var result = Zxcvbn.Core.EvaluatePassword(Password);
            int Result = result.Score;
            return Result;
        }


    }
}
