using System.ComponentModel.DataAnnotations;

namespace KachingExpress.Models
{
    public class ResetPassword
    {
        [Required(ErrorMessage = "Email field cannot be empty!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password field cannot be empty!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password field cannot be empty!")]
        [Compare("Password", ErrorMessage = "Passwords are not the same!")]
        public string ConfirmPassword { get; set;}

        
    }
}
