using System.ComponentModel.DataAnnotations;

namespace NouveauP7API.Models
{
    public class RegisterUser
    {

        public string? UserName { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public bool? EmailConfirmed { get; set; }

        [Required]
        public string? Fullname { get; set; }

        [Required]
        public string? Role { get; set; }
        [Required]
        public string? Password { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }



    }

}
