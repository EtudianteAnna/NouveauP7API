using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace NouveauP7API.Models
{
    public class User :IdentityUser
    {
        [Required]
        public override string? UserName { get; set; }

        [Required]
        public  override string ? PasswordHash { get; set; }

        [Required]
#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
        public string Fullname { get; set; }
#pragma warning restore CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.

        [Required]
        public string? Role { get; set; }
        public override string? Email { get; set; }
        public override bool EmailConfirmed { get;  set; }


    }
}