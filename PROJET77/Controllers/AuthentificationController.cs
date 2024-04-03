using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NouveauP7API.Models;
using NouveauP7API.Repositories;


[ApiController]
[Route("api/[controller]")]
public class AuthentificationController : ControllerBase
{
    //private readonly ILogger<AuthentificationController> _logger=new Logger<AuthentificationController>();//
    private readonly IUserRepository _userRepository;
    private readonly IJwtFactory _jwtFactory;
    private readonly UserManager<User> _userManager;


    public AuthentificationController(
   IUserRepository userRepository,
        IJwtFactory jwtFactory,
        UserManager<User> userManager)
    {

        _userRepository = userRepository;
        _jwtFactory = jwtFactory;
        _userManager = userManager;

    }
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        // Validation du modèle
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

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

        // Génération du token JWT
        var token = _jwtFactory.GeneratedEncodedToken(user);

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


        // Générer le token JWT pour l'utilisateur fictif
        var token = _jwtFactory.GeneratedEncodedToken(dummyUser); // Utilisez dummyUser ou un autre utilisateur fictif selon vos besoins

        // Retourner le token dans la réponse
        return Ok(new { Token = token });
    }

}



