using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NouveauP7API.Models;

namespace NouveauP7API.Repositories
{
    public class JwtFactory : IJwtFactory
    {
        private readonly JwtSettings _jwtSettings;

        public JwtFactory(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        public virtual string GeneratedEncodedToken(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                // Ajouter d'autres revendications si nécessaire
            };

            return GeneratedEncodedToken(claims);
        }

        public string GeneratedEncodedToken((string Username, string Email, string Password) newUser)
        {
            if (string.IsNullOrEmpty(newUser.Username) || string.IsNullOrEmpty(newUser.Email) || string.IsNullOrEmpty(newUser.Password))
            {
                throw new ArgumentException("Username, Email, and Password must not be empty");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, newUser.Username),
                new Claim(ClaimTypes.Email, newUser.Email),
                // Ajouter d'autres revendications si nécessaire
            };

            return GeneratedEncodedToken(claims);
        }

        public string GeneratedEncodedToken(ClaimsPrincipal user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var claims = new List<Claim>();
            claims.AddRange(user.Claims);

            return GeneratedEncodedToken(claims);
        }

        private string GeneratedEncodedToken(IEnumerable<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public object GeneratedEncodedToken(object user)
        {
            throw new NotImplementedException();
        }
    }
}
