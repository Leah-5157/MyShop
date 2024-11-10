using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class User
    {
        public int userId { get; set; }
        [Required,EmailAddress]
        public string UserName { get; set; }
        [StringLength(12, ErrorMessage = "password must be between 8 till 12 tags", MinimumLength = 8), Required]
        public string Password { get; set; }
       
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
