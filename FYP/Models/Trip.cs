using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace FYP.Models
{
   public class Trip
   {
      // TODO L08 Task 3 - Specify [Required] for some properties
      [Required]
      public int Id { get; set; }
      [Required]
      public string Title { get; set; }
      [Required]
      public string City { get; set; }
      [Required]
      public DateTime TripDate { get; set; }
      [Required]
      public int Duration { get; set; }
      [Required]
      public double Spending { get; set; }
      [Required]
      public string Story { get; set; }
      [Required]
      public IFormFile Photo { get; set; }
      [Required]
      public string Picture { get; set; }
      public string SubmittedBy { get; set; }    
   }
}

