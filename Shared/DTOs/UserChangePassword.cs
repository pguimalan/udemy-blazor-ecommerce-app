using System.ComponentModel.DataAnnotations;

namespace BlazorEcommerceApp.Shared.DTOs
{
    public class UserChangePassword
    {
        [Required, StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage ="Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
