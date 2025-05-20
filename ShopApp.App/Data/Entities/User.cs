namespace ShopApp.Data.Entities;


public interface IUser
{
    int UserId { get; set; }
    string Username { get; set; }
    string PasswordHash { get; set; } 
    string PasswordSalt { get; set; } 
    string IdEmployee { get; set; }
}

public class User: IUser
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public string IdEmployee { get; set; }
}