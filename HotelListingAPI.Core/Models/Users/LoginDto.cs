using System.ComponentModel.DataAnnotations;

namespace HotelsListingAPI.Core.Models.Users
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
