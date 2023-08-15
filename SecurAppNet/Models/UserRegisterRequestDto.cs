using System.ComponentModel.DataAnnotations;

namespace SecurAppNet.Models
{
    public class UserRegisterRequestDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
        public required string ConfirmPassword { get; set; }    
    }
}