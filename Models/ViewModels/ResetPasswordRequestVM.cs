using Microsoft.Build.Framework;

namespace Cinema.Models.ViewModels
{
    public class ResetPasswordRequestVM
    {
        [Required]
        public string UserNameOrEmail { get; set; } = null!;
        public string ResetMethod { get; set; } = string.Empty;
    }
}
