
using System.ComponentModel.DataAnnotations;

namespace FYP.Models
{
   public class UserLogin
   {
      [Required(ErrorMessage = "Please enter User ID")]
      public string UserID { get; set; }

      [Required(ErrorMessage = "Please enter Password")]
      public string Password { get; set; }
   }
}

