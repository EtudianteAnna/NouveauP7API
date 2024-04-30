using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NouveauP7API.Models;
using NouveauP7API.Repositories;
using System.Security.Claims;
namespace NouveauP7API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthentificationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtFactory _jwtFactory;

        public AuthentificationController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IJwtFactory? jwtFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtFactory = jwtFactory ?? throw new ArgumentNullException(nameof(jwtFactory));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return Unauthorized();
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordValid)
            {
                return Unauthorized();
            }

            bool emailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!emailConfirmed)
            {
                return BadRequest("L'adresse email n'est pas confirmée.");
            }

            // Initialiser les propriétés de date
            user.UpdatedAt = DateTime.UtcNow;

            // Récupérer les rôles de l'utilisateur
            var userRolesList = await _userManager.GetRolesAsync(user);

            // Créer les revendications (claims) pour le jeton JWT
            var claims = (await _userManager.GetClaimsAsync(user)).ToList();

            // Ajouter les rôles de l'utilisateur aux revendications
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            claims.Add(new Claim(ClaimTypes.Role, user.Role));

            // Ajouter l'ID de l'utilisateur aux revendications
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

            var token = await _jwtFactory.GeneratedEncodedTokenAsync(claims);
            return Ok(new { Token = token });
        }

        [AllowAnonymous]
        [HttpGet("token")]
        public async Task<IActionResult> GetToken()
        {
            try
            {
                var dummyUser = new User
                {
                    UserName = "dummyUsername",
                    Email = "dummyEmail@example.com",
                    UpdatedAt = DateTime.UtcNow,
                };

                // Créer les revendications (claims) pour le jeton JWT
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, dummyUser.UserName),
                new Claim(ClaimTypes.Email, dummyUser.Email),
                new Claim(ClaimTypes.NameIdentifier, dummyUser.Id)
            };

                var token = await _jwtFactory.GeneratedEncodedTokenAsync(claims);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                // Gérer l'exception et renvoyer une réponse HTTP appropriée
                return StatusCode(StatusCodes.Status500InternalServerError, "Une erreur s'est produite lors de la génération du jeton.");
            }
        }
    }
}