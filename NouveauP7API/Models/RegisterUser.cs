using System.ComponentModel.DataAnnotations;

namespace NouveauP7API.Models
{
    public class RegisterUser
    {

        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public string? Email { get; set; }

    }

}
