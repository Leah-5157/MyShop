using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Entities;
using Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class UsersController : ControllerBase
    {
        IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [HttpPost("Login")]
        public ActionResult Login([FromQuery] string UserName, [FromQuery] string Password)
        {

               User user = _userService.Login(UserName, Password);
               if (user!= null)
                        return Ok(user);
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

        public ActionResult Post([FromBody] User user)
        {
            int score = Password(user.Password);
            if (score <= 2)
                return NoContent();
            user = _userService.Post(user);
            return CreatedAtAction(nameof(Get), new { id = user.userId }, user);

        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public ActionResult<User> Put(int id, [FromBody] User userToUpdate)
        {
             User user= _userService.Put(id, userToUpdate);
            if (user != null)
                return Ok(user);
            return NoContent();
            


        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
