using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace _01SampleAuth.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required,Display(Name= "First Name")]
        public string FirstName { get; set; } = String.Empty;
#nullable disable
        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}
