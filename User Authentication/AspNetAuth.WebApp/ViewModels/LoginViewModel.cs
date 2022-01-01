using System.ComponentModel.DataAnnotations;

namespace AspNetAuth.WebApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Minimum password length is 6 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }
    }
}