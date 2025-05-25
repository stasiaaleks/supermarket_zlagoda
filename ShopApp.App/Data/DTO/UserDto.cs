namespace ShopApp.Data.DTO;

public class UserDto
{
    public int? UserId { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public string IdEmployee { get; set; }
}

public class CreateUserDto 
{
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public string IdEmployee { get; set; }
}