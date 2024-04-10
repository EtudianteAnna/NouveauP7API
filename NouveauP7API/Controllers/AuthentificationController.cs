using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NouveauP7API.Models;
using NouveauP7API.Repositories;

namespace NouveauP7API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthentificationController : ControllerBase
    {

        private readonly ILogger<AuthentificationController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IJwtFactory jwtFactory;

        public AuthentificationController(ILogger<AuthentificationController> logger, UserManager<User> userManager, IJwtFactory jwtFactory)
        {

            _logger = logger;
            _userManager = userManager;
            this.jwtFactory = jwtFactory;

        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // Recherche de l'utilisateur par nom d'utilisateur
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return Unauthorized();
            }

            // Vérification du mot de passe
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordValid)
            {
                return Unauthorized();
            }

            // Vérification de l'adresse email confirmée
            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!isEmailConfirmed)
            {
                return BadRequest("L'adresse email n'est pas confirmée.");
            }

            // Génération du token JWT à l'aide de la méthode existante
            var token = jwtFactory.GeneratedEncodedToken(user);

            // Retourner le token en cas de succès
            return Ok(new { Token = token });
        }

        // Endpoint pour récupérer le token en cas d'erreur 401
        [AllowAnonymous]
        [HttpGet("token")]
        public IActionResult GetToken()
        {
            // Créer un utilisateur fictif
            var dummyUser = new User
            {
                UserName = "dummyUsername",
                Email = "dummyEmail@example.com",
                // Autres propriétés de l'utilisateur
            };

            // Générer le token JWT pour l'utilisateur fictif à l'aide de la méthode existante
            var token = jwtFactory.GeneratedEncodedToken(dummyUser);

            // Retourner le token dans la réponse
            return Ok(new { Token = token });
        }
    }
}
//Cette modification renforce la sécurité en empêchant les utilisateurs non confirmés d'accéder à l'application via l'endpoint login.
// il est recommandé de garder la méthode GetToken asynchrone, même si elle n'effectue pas d'opération longue. Cela permettra de maintenir une cohérence dans votre API et de vous préparer à une éventuelle évolution future de cette méthode.
