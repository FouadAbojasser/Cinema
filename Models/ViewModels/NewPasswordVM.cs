using System.ComponentModel.DataAnnotations;

namespace Cinema.Models.ViewModels
{
    public class NewPasswordVM
    {
        public string ApplicationUserId { get; set; } = null!;
        public string Token { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = null!;
        [Required]
        [Compare(nameof(NewPassword))]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; } = null!;
    }
}
