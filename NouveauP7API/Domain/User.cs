using System.ComponentModel.DataAnnotations;

namespace NouveauP7API.Domain
{
    public class User 
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
        }
           
    }
}