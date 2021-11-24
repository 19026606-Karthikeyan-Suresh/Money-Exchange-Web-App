using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace FYP.Models
{
   public class Candidate
   {
      public int RegNo { get; set; } // int's Default is Required

      [Required]
      public string CName { get; set; }

      [Required]
      public string Gender { get; set; }

      public double Height { get; set; } // double's Default is Required

      public DateTime BirthDate { get; set; } // DateTime's Default is Required

      [Required]
      public string Race { get; set; }

      public bool Clearance { get; set; } // bool's Default is False

      [Required]
      public IFormFile Photo { get; set; }

      public string PicFile { get; set; } // PicFile derives from Photo
   }
}


