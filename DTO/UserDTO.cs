using System.ComponentModel.DataAnnotations;

namespace DTO
{
    
    public record UserDTO(int Id, [Required][EmailAddress] string UserName,[Required]string FirstName, string? LastName);
    public record PostUserDTO([Required][EmailAddress]string UserName, [Required]string FirstName, string? LastName,[Required]string Password);


}

    