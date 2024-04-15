using NouveauP7API.Models;
using System.Security.Claims;

namespace NouveauP7API.Repositories

{

    public interface IJwtFactory
    {
       Task< string> GeneratedEncodedTokenAsync(User user);
        string GeneratedEncodedTokenAsync((string Username, string Email, string Password) newUser);
        string GeneratedEncodedTokenAsync(ClaimsPrincipal user);

    }
}
