using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NouveauP7API.Models; // Assurez-vous que vous importez le bon namespace pour vos modèles
using NouveauP7API.Repositories;

namespace NouveauP7API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtFactory _jwtFactory;

        public RegisterController(IUserRepository userRepository, IJwtFactory jwtFactory)
        {
            _userRepository = userRepository;
            _jwtFactory = jwtFactory;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUser model)
        {
            try
            {
                // Créez un nouvel objet User à partir des données de RegisterUser
                var newUser = new User
                {
                    UserName = model.UserName,
                    PasswordHash = model.Password,
                    Email = model.Email
                    // Ajoutez d'autres propriétés si nécessaire
                };

                // Ajoutez le nouvel utilisateur
                await _userRepository.AddAsync(newUser);

                // Générez le token JWT pour le nouvel utilisateur
                var token = _jwtFactory.GeneratedEncodedToken(newUser);

                // Retournez le token JWT dans le résultat
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                // Retournez une réponse BadRequest en cas d'erreur
                return BadRequest(ex.Message);
            }
        }
    }
}
