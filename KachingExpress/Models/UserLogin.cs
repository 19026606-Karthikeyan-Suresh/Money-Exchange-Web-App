
using System.ComponentModel.DataAnnotations;

namespace KachingExpress.Models
{
   public class UserLogin
   {
      [Required(ErrorMessage = "Email Address cannot be empty!")]
      public string UserID { get; set; }

      [Required(ErrorMessage = "Password cannot be empty!")]
      public string Password { get; set; }
   }
}

