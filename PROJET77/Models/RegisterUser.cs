using System.ComponentModel.DataAnnotations;

namespace NouveauP7API.Models
{
    public class RegisterUser
    {
        public int Id { get; set; }
        [Required]

        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public string? Email { get; set; }

    }

}
