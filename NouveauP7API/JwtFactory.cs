using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NouveauP7API.Domain;
using NouveauP7API.Repositories;

namespace NouveauP7API.Repositories
{
    public class JwtFactory : IJwtFactory
    {
        private readonly NouveauP7API.Domain.JwtSettings _jwtSettings; // Spécifiez le nom complet du JwtSettings pour éviter l'ambiguïté

        public JwtFactory(NouveauP7API.Domain.JwtSettings jwtSettings) // Utilisez le nom complet du JwtSettings
        {
            _jwtSettings = jwtSettings;
        }

        public string GenerateEncodedToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    // Ajouter d'autres claims selon les besoins
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GeneratedEncodedToken(User user)
        {
            // Implémentez cette méthode en fonction de vos besoins
            return GenerateEncodedToken(user);
        }

        public string GeneratedEncodedToken((string Username, string Email, string Password) newUser)
        {
            throw new NotImplementedException();
        }

        public string GeneratedEncodedToken(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        // Ajoutez l'implémentation des autres méthodes de l'interface ici
    }
}
