using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace NouveauP7API.Domain
{
    public class User :IdentityUser
    {


        [Required]
        public override string Id { get; set; }

        [Required]
        public override string UserName { get; set; }

        [Required]
        public  override string  PasswordHash { get; set; }

        [Required]
        public string Fullname { get; set; }

        [Required]
        public string? Role { get; set; }
        public override string Email { get;  set; }
        public override bool EmailConfirmed { get;  set; }

        public User()
        {
            Id = Guid.NewGuid().ToString(); // Initialisation de Id avec une nouvelle valeur unique
        }
           
    }
}