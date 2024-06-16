using Microsoft.AspNetCore.Identity;

namespace MinWebApi.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public int Age { get; set; }
        public string Country { get; set; }
    }
}
