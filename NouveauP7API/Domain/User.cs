using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace NouveauP7API.Domain
{
    public class User : IdentityUser
    {


        [Required]
        public override string UserName { get; set; }

        [Required]
        public override string Email { get; set; }

        [Required]
        public override string PasswordHash { get; set; }

        [Required]
        public string Fullname { get; set; }

        [Required]
        public string? Role { get; set; }

        // Ajoutez d'autres propriétés si nécessaire

        // Constructeur par défaut sans paramètres
        public User() : base()
        {
            // Initialisations supplémentaires si nécessaires
        }

        // Constructeur avec paramètres pour faciliter la création d'instances
        public User(string username, string email, string password)
        {
            UserName = username;
            Email = email;
        }
    }
}