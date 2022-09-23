using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace _01SampleAuth.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Display(Name= "First Name")]
        public string FirstName { get; set; } = String.Empty;
#nullable disable
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}
