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

        public async Task<string> GeneratedEncodedTokenAsync(List<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<string> GeneratedEncodedTokenAsync(ClaimsPrincipal user)
        {
            var claims = new List<Claim>();
            claims.AddRange(user.Claims);
            return await GeneratedEncodedTokenAsync(claims);
        }

        public async Task<string> GeneratedEncodedTokenAsync(User user, bool emailConfirmed)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("EmailConfirmed", emailConfirmed.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiryInMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Task<string> GeneratedEncodedTokenAsync((string Username, string Email, string Password, bool EmailConfirmed) newUser)
        {
            throw new NotImplementedException();
        }
    }
}