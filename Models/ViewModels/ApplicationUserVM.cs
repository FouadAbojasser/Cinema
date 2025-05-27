using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Cinema.Models.ViewModels
{
    public class ApplicationUserVM
    {
        public ApplicationUser ApplicationUser { get; set; } = null!;
        public List<string> UserRoles { get; set; } = [];
        public List<string> AvailableRoles { get; set; } = [];
    }
}
