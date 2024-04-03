using NouveauP7API.Models;
using System.Security.Claims;

namespace NouveauP7API.Repositories

{

    public interface IJwtFactory
    {
        string GeneratedEncodedToken(User user);
        string GeneratedEncodedToken((string Username, string Email, string Password) newUser);
        string GeneratedEncodedToken(ClaimsPrincipal user);
        object GeneratedEncodedToken(object user);
    }
}
