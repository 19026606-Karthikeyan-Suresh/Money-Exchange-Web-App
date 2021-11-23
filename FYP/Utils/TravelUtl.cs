using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class TravelUtl
{
   public static string Abbreviate(this string story)
   {
      if (story.Length < 100)
      {
         return story;
      }
      else
      {
         return story.Substring(0, 100) + " ... ";
      }
   }
}
