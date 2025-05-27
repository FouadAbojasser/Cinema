using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Cinema.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateOnly DoB {  get; set; }
        public string Country { get; set; } = string.Empty;
        public string Image {  get; set; }= string.Empty;
        public List<string> UserRoles { get; set; } = [];
        public List<MovieReviews> movieReviews { get; set; } =  [];
    }
}