
using System.ComponentModel.DataAnnotations;

namespace MoneyExchangeWebApp.Models
{
   public class UserLogin
   {
      [Required(ErrorMessage = "Username cannot be empty!")]
      public string UserID { get; set; }

      [Required(ErrorMessage = "Password cannot be empty!")]
      public string Password { get; set; }
   }
}

