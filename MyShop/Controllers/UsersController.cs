using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Entities;
using Services;
using DTO;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class UsersController : ControllerBase
    {
        IUserService _userService;
        IMapper _mapper;
        public UsersController(IUserService userService,IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
      
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromQuery] string UserName, [FromQuery] string Password)
        {
            User user = await _userService.Login(UserName, Password);
               if (user!= null) {
                UserDTO userDTO = _mapper.Map<User, UserDTO>(user);
                        return Ok(userDTO);}
               return NoContent();
        }
        [HttpPost("Password")]
        public int Password([FromBody] string Password)
        {
            int score = _userService.Password(Password);
            return score;
        }
        // POST api/<UsersController>
        [HttpPost]

        public async Task<ActionResult> Post([FromBody] PostUserDTO postUserDTO)
        {
            User user = _mapper.Map<PostUserDTO, User>(postUserDTO);
            int score = Password(user.Password);
            if (score <= 2)
                return BadRequest();
            user = await _userService.Post(user);
            UserDTO userDTO = _mapper.Map<User, UserDTO>(user);
            return CreatedAtAction(nameof(Get), new { id = userDTO.Id }, userDTO);

        }

        // PUT api/<UsersController>/5
        //[HttpPut("{id}")]
        //public async Task  Put(int id, [FromBody] PostUserDTO userToUpdate)
        //{
        //    await _userService.Put(id,_mapper.Map<PostUserDTO, User>(userToUpdate));

        //}
        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] PostUserDTO userToUpdateDTO)

        {

            User user = _mapper.Map<PostUserDTO, User>(userToUpdateDTO);

            int score = Password(user.Password);
            if (score <= 2)
                return BadRequest();

            await _userService.Put(id, user);
            UserDTO userDTO = _mapper.Map<User, UserDTO>(user);

            return Ok(userDTO);

        }
    }
}
