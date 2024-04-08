using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NouveauP7API.Models;
using NouveauP7API.Repositories;

[ApiController]
[Route("api/[controller]")]
public class AuthentificationController : ControllerBase
{
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<AuthentificationController> _logger;
    private readonly UserManager<User> _userManager;
    private readonly JwtFactory _jwtFactory;

    public AuthentificationController(JwtSettings jwtSettings, ILogger<AuthentificationController> logger, UserManager<User> userManager )
    {
        _jwtSettings = jwtSettings;
        _logger = logger;
        _userManager = userManager;

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

        // Génération du token JWT à l'aide de la méthode existante
        var token = _jwtFactory.GeneratedEncodedToken(user);

        // Retourner le token en cas de succès
        return Ok(new { Token = token });
    }

    // Endpoint pour récupérer le token en cas d'erreur 401
    [AllowAnonymous]
    [HttpGet("token")]
    public async Task<IActionResult> GetToken()
    {
        // Créer un utilisateur fictif
        var dummyUser = new User
        {
            UserName = "dummyUsername",
            Email = "dummyEmail@example.com",
            // Autres propriétés de l'utilisateur
        };

        // Générer le token JWT pour l'utilisateur fictif à l'aide de la méthode existante
        var token = _jwtFactory.GeneratedEncodedToken(dummyUser);

        // Retourner le token dans la réponse
        return Ok(new { Token = token });
    }
}


