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
        UserService service = new();
        
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

               User user = service.Login(UserName, Password);
               if (user!= null)
                        return Ok(user);
               return NoContent();
        }

        // POST api/<UsersController>
        [HttpPost]

        public ActionResult Post([FromBody] User user)
        {
            user = service.Post(user);
            return CreatedAtAction(nameof(Get), new { id = user.userId }, user);

        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public ActionResult<User> Put(int id, [FromBody] User userToUpdate)
        {
             User user=service.Put(id, userToUpdate);
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
