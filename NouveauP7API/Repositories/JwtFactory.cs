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

        public async Task<string> GeneratedEncodedTokenAsync(User user)
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

            return await GenerateEncodedTokenAsync(claims);
        }

        public async Task<string> GenerateEncodedTokenAsync(IEnumerable<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
#pragma warning disable CS8604 // Existence possible d'un argument de référence null.
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
#pragma warning restore CS8604 // Existence possible d'un argument de référence null.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token =  tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GeneratedEncodedTokenAsync((string Username, string Email, string Password) newUser)
        {
            if (string.IsNullOrEmpty(newUser.Username) || string.IsNullOrEmpty(newUser.Email) || string.IsNullOrEmpty(newUser.Password))
            {
                throw new ArgumentException("Username, Email, and Password must not be empty");
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, newUser.Username),
                new Claim(ClaimTypes.Email, newUser.Email),
                new(ClaimTypes.Upn, newUser.Password)
            };

            return GeneratedEncodedToken(claims);
        }

        public string GeneratedEncodedTokenAsync(ClaimsPrincipal user)
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
#pragma warning disable CS8604 // Existence possible d'un argument de référence null.
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
#pragma warning restore CS8604 // Existence possible d'un argument de référence null.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}