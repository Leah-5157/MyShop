using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Entities;
using Services;
using DTO;
using AutoMapper;
using MyShop;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UsersController : ControllerBase
    {
        IUserService _userService;
        IMapper _mapper;
        ILogger<UsersController> _logger;
        private readonly TokenService _tokenService;
        public UsersController(IUserService userService, IMapper mapper, ILogger<UsersController> logger, TokenService tokenService)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
            _tokenService = tokenService;
        }
      
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
       
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            User user = await _userService.Login(request.UserName, request.Password);
            if (user != null)
            {
                UserDTO userDTO = _mapper.Map<User, UserDTO>(user);
                _logger.LogInformation($"User {user.Id} enter to the application");
                // Generate JWT
                var token = _tokenService.GenerateToken(user.UserName, "User");
                Response.Cookies.Append("jwt_token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(60)
                });
                return Ok(userDTO);
            }
            return Unauthorized("Invalid username or password");
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
            if (user == null) {
                return BadRequest();
            }
            UserDTO userDTO = _mapper.Map<User, UserDTO>(user);
            return CreatedAtAction(nameof(Get), new { id = userDTO.Id }, userDTO);

        }
        [Authorize]
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
