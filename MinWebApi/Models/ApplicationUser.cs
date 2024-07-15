using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace MinWebApi.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public int Age { get; set; }
        public string Country { get; set; }
    }
}
