using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NouveauP7API.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace NouveauP7API.Repositories.JwtFactory
{
    internal class JwtFactory : IJwtFactory
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly UserManager<User> _userManager;

        public JwtFactory(IConfiguration configuration, UserManager<User> userManager)
        {
            _secretKey = configuration["JwtSettings:SecretKey"];
            _issuer = configuration["JwtSettings:Issuer"];
            _audience = configuration["JwtSettings:Audience"];
            _userManager = userManager;
        }

        public string GeneratedEncodedToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    // Ajoutez d'autres revendications selon les besoins, comme l'ID utilisateur, les rôles, etc.
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Durée de validité du jeton
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GeneratedEncodedToken((string Username, string Email, string Password) newUser)
        {
            // Implémentez la logique pour générer un jeton JWT pour un nouvel utilisateur
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, newUser.Username),
                    new Claim(ClaimTypes.Email, newUser.Email),
                    // Ajoutez d'autres revendications selon les besoins, comme l'ID utilisateur, les rôles, etc.
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Durée de validité du jeton
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GeneratedEncodedToken(ClaimsPrincipal user)
        {
            // Implémentez la logique pour générer un jeton JWT pour un utilisateur existant
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = user.Identity as ClaimsIdentity,
                Expires = DateTime.UtcNow.AddHours(1), // Durée de validité du jeton
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
