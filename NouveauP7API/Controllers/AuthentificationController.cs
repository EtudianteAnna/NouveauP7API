using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NouveauP7API.Models;
using NouveauP7API.Repositories;


[ApiController]
[Route("api/[controller]")]
public class AuthentificationController : ControllerBase
{
    private readonly ILogger<AuthentificationController> _logger=new Logger<AuthentificationController>();
    private readonly IUserRepository _userRepository;
    private readonly IJwtFactory _jwtFactory;


    public AuthentificationController(IUserRepository userRepository, IJwtFactory jwtFactory)
    {
        
        _userRepository = userRepository;
        _jwtFactory = jwtFactory;

    }
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        _logger.LogInformation($"Tentative de connexion : {model.Username}");

        // Validation du modèle
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        string newToken = null; // Déclarez la variable newToken localement

        var user = await _userRepository.GetUserByCredentialsAsync(model.Username);

        if (user == null)
        {
            _logger.LogWarning($"Utilisateur avec le nom d'utilisateur {model.Username} non trouvé.");

            // Créer un nouvel utilisateur avec le modèle
            var newUser = (model.Username, model.Email, model.Password);

            // Ajouter le nouvel utilisateur à la base de données
            await _userRepository.AddUserAsync(newUser);

            // Générer le jeton JWT pour le nouvel utilisateur
            newToken = (string)_jwtFactory.GeneratedEncodedToken(newUser);

            // Retourner la réponse avec le jeton
            return Ok(new { Message = "Utilisateur créé et connecté avec succès", Token = newToken });
        }

        // Vérifier le mot de passe avec le CustomPasswordHasher
        // bool passwordMatch = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);//

        //if (!passwordMatch)
        //{
        //    _logger.LogWarning($"Mot de passe incorrect pour l'utilisateur {model.Username}.");
        //    return BadRequest("Mot de passe incorrect");
        //}

        // Générer le jeton JWT pour l'utilisateur existant
        newToken = (string)_jwtFactory.GeneratedEncodedToken(User);

        _logger.LogInformation($"Connexion réussie pour l'utilisateur {model.Username}.");

        // Retourner la réponse avec le jeton
        return Ok(new { Message = "Connexion réussie", Token = newToken });
    }
}

