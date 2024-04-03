/*using Microsoft.AspNetCore.Identity;

namespace NouveauP7API.Domain;
public class CustomPasswordHasher : PasswordHasher<IdentityUser>, IPasswordHasher<IdentityUser>
{
    public override string HashPassword(IdentityUser user, string password)
    {
        // Génère un hachage de mot de passe avec BCrypt
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        return hashedPassword;
    }

    public  override PasswordVerificationResult VerifyHashedPassword(IdentityUser user, string hashedPassword, string providedPassword)
    {
        // Vérifie le mot de passe fourni avec le hachage stocké en utilisant BCrypt
        bool passwordMatch = BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);

        return passwordMatch ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
    }
}*/



