using System.ComponentModel.DataAnnotations;

namespace Cinema.Models.ViewModels
{
    public class RegisterVM 
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Required]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;
        public bool RememberMe { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? DoB { get; set; } 
        public string? Country { get; set; } = string.Empty;

    }
}
