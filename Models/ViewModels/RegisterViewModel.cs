using System.ComponentModel.DataAnnotations;

namespace ThirdWebseite.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required.")]
        public string? UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required.")]
        public string? Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required.")]
        public string? Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Confirm Password is required.")]
        public string? ConfirmPassword { get; set; }

        public string? Role { get; set; }
    }
}
