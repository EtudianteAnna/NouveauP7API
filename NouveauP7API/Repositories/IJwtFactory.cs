using NouveauP7API.Domain;
using System.Security.Claims;

namespace NouveauP7API.Repositories

{

    public interface IJwtFactory
    {
        string GeneratedEncodedToken(User user);
        string GeneratedEncodedToken((string Username, string Email, string Password) newUser);
        string GeneratedEncodedToken(ClaimsPrincipal user);
    }
}
