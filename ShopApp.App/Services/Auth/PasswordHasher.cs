using System.Security.Cryptography;

namespace ShopApp.Services.Auth;

public interface IPasswordHasher
{
    (string hash, string salt) HashPassword(string password);
    bool VerifyPassword(string password, string storedHash, string storedSalt);
}

public class Pbkdf2PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16; 
    private const int HashSize = 32; 
    private const int Iterations = 100_000;
    
    public (string hash, string salt) HashPassword(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        var saltBytes = new byte[SaltSize];
        rng.GetBytes(saltBytes);

        var hashBytes = Rfc2898DeriveBytes.Pbkdf2(
            password,
            saltBytes,
            Iterations,
            HashAlgorithmName.SHA256,
            HashSize);

        return (Convert.ToBase64String(hashBytes), Convert.ToBase64String(saltBytes));
    }

    public bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        var saltBytes = Convert.FromBase64String(storedSalt);
        var hashBytes = Convert.FromBase64String(storedHash);

        var testHash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            saltBytes,
            Iterations,
            HashAlgorithmName.SHA256,
            hashBytes.Length);

        return CryptographicOperations.FixedTimeEquals(testHash, hashBytes);
    }
}