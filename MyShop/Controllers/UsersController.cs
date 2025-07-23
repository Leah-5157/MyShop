using Microsoft.AspNetCore.Mvc;
using Entities;
using Services;
using DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace MyShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;
        private readonly TokenService _tokenService;
        public UsersController(IUserService userService, IMapper mapper, ILogger<UsersController> logger, TokenService tokenService)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
            _tokenService = tokenService;
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.LoginAsync(request.UserName, request.Password);
            if (user != null)
            {
                var userDTO = _mapper.Map<User, UserDTO>(user);
                _logger.LogInformation($"User {user.Id} enter to the application");
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
        public int GetPasswordStrength([FromBody] string password)
        {
            return _userService.GetPasswordStrength(password);
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] PostUserDTO postUserDTO)
        {
            var user = _mapper.Map<PostUserDTO, User>(postUserDTO);
            int score = GetPasswordStrength(user.Password);
            if (score <= 2)
                return BadRequest();
            user = await _userService.RegisterAsync(user);
            if (user == null)
                return BadRequest();
            var userDTO = _mapper.Map<User, UserDTO>(user);
            return CreatedAtAction(nameof(Register), new { id = userDTO.Id }, userDTO);
        }

        [Authorize]
        [HttpPut("{id}/UpdatePassword")]
        public async Task<ActionResult> UpdatePassword(int id, [FromBody] UpdatePasswordRequest request)
        {
            var salt = await _userService.GetSaltByUserName(request.UserName);
            if (salt == null)
                return BadRequest("Please check the form fields.");
            var user = await _userService.LoginAsync(request.UserName, request.CurrentPassword);
            if (user == null)
                return BadRequest("Please check the form fields.");
            user.Password = _userService.HashPassword(request.NewPassword, salt);
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            await _userService.UpdateAsync(id, user);
            var userDTO = _mapper.Map<User, UserDTO>(user);
            return Ok(userDTO);
        }
    }
}
