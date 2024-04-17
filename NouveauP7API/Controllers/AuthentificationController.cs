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
        private readonly UserManager<User> _userManager;
        private readonly IJwtFactory _jwtFactory;

        public AuthentificationController(UserManager<User> userManager, IJwtFactory jwtFactory)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
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

            var token =  _jwtFactory.GeneratedEncodedTokenAsync((user.UserName, user.Email, model.Password, emailConfirmed));
            return Ok(new { Token = token });
        }

        [AllowAnonymous]
        [HttpGet("token")]
        public async Task<IActionResult> GetToken()
        {
            var dummyUser = new User
            {
                UserName = "dummyUsername",
                Email = "dummyEmail@example.com"
            };

            var token =  _jwtFactory.GeneratedEncodedTokenAsync((dummyUser.UserName, dummyUser.Email, "dummyPassword", true));
            return Ok(new { Token = token });
        }
    }
}