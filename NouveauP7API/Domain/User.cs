using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace NouveauP7API.Domain
{
    public class User :IdentityUser
    {


        [Required]
        public string Id { get; set; }

        [Required]
        public  string UserName { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Fullname { get; set; }

        [Required]
        public string? Role { get; set; }
        public string Email { get; internal set; }
        public bool EmailConfirmed { get; internal set; }

        public User()
        {
            Id = Guid.NewGuid().ToString(); // Initialisation de Id avec une nouvelle valeur unique
        }
           
    }
}