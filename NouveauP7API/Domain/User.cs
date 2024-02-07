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

        // Ajoutez d'autres propri�t�s si n�cessaire

        // Constructeur par d�faut sans param�tres
        public User() : base()
        {
            // Initialisations suppl�mentaires si n�cessaires
        }

        // Constructeur avec param�tres pour faciliter la cr�ation d'instances
        public User(string username, string email, string password)
        {
            UserName = username;
            Email = email;
        }
    }
}