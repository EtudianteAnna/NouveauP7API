﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NouveauP7API.Models;
using NouveauP7API.Repositories;

namespace NouveauP7API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        
        private readonly IJwtFactory _jwtFactory;
        private readonly UserManager<User> _userManager;
       

        public RegisterController(IJwtFactory jwtFactory, UserManager<User> userManager)
        {
            _jwtFactory = jwtFactory;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUser model)
        {
            try
            {
                // Déclarer explicitement la variable newUser
                User newUser;
                // Initialiser newUser
#pragma warning disable CS8601 // Existence possible d'une assignation de référence null.
#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
                newUser = new User


                {
                    UserName = model.UserName,
                    Email = model.Email,
                    EmailConfirmed = true,
                    Fullname = model.Fullname,
                    Role = "User",
                    PasswordHash = _userManager.PasswordHasher.HashPassword(null, model.Password)
                    // Ajoutez d'autres propriétés si nécessaire
                };
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
#pragma warning restore CS8601 // Existence possible d'une assignation de référence null.

                // Ajoutez le nouvel utilisateur avec hachage du mot de passe par UserManager
                var result = await _userManager.CreateAsync(newUser, model.Password);

                if (result.Succeeded)
                {
                    // Générer le token JWT pour le nouvel utilisateur, en incluant l'état de confirmation de l'email
                    var token = await _jwtFactory.GeneratedEncodedTokenAsync(newUser, newUser.EmailConfirmed);

                    // Retournez le token JWT dans le résultat
                    return Ok(new { Token = token });
                }
                else
                {
                    // Gérez les erreurs de création de l'utilisateur
                    return BadRequest(result.Errors);
                }
            }
            catch (Exception ex)
            {
                // Retournez une réponse BadRequest en cas d'erreur
                return BadRequest(ex.Message);
            }
        }
    }
}

