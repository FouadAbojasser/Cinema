using Microsoft.AspNetCore.Identity;

namespace Cinema.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateOnly DoB {  get; set; }
        public string Country { get; set; } = string.Empty;
        List<Review> Reviews { get; set; } =  [];

    }
}