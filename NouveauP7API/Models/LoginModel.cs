using System.ComponentModel.DataAnnotations;


namespace NouveauP7API.Models
{
    public class LoginModel
    {

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
