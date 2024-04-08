using Microsoft.AspNetCore.Identity;
using NouveauP7API.Models;
using System.Security.Cryptography;

public class CustomPasswordHasher : IPasswordHasher<User>
{
    public string HashPassword(User user, string password)
    {
        // Générer un sel aléatoire
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // Hasher le mot de passe avec le sel
        string hashedPassword = HashPasswordWithSalt(password, salt);

        return hashedPassword;
    }

    public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
    {
        // Extraire le sel du mot de passe haché
        byte[] salt = ExtractSaltFromHashedPassword(hashedPassword);

        // Rehasher le mot de passe fourni avec le sel
        string rehashedPassword = HashPasswordWithSalt(providedPassword, salt);

        // Comparer les mots de passe hashés
        if (hashedPassword == rehashedPassword)
        {
            return PasswordVerificationResult.Success;
        }
        else
        {
            return PasswordVerificationResult.Failed;
        }
    }

    private string HashPasswordWithSalt(string password, byte[] salt)
    {
        // Utiliser PBKDF2-SHA256 pour hasher le mot de passe avec le sel
        Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
        byte[] hash = pbkdf2.GetBytes(256 / 8);

        // Concaténer le sel et le hash pour stocker le mot de passe haché
        return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
    }

    private byte[] ExtractSaltFromHashedPassword(string hashedPassword)
    {
        // Extraire le sel du mot de passe haché
        string[] parts = hashedPassword.Split(':');
        return Convert.FromBase64String(parts[0]);
    }
}



