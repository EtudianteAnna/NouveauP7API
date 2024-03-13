using NouveauP7API.Domain;
using NouveauP7API.Repositories;
using System.Security.Claims;

namespace NouveauP7API
{
    internal class JwtFactory : IJwtFactory
    {
        public string GeneratedEncodedToken(User user)
        {
            throw new NotImplementedException();
        }

        public object GeneratedEncodedToken((string Username, string Email, string Password) newUser)
        {
            throw new NotImplementedException();
        }

        public object GeneratedEncodedToken(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }
    }
}