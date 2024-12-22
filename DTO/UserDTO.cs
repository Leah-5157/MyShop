namespace DTO
{
    public record UserDTO(int Id, string UserName, string FirstName, string? LastName);
    public record PostUserDTO(string UserName, string FirstName, string? LastName,string Password);


}

   