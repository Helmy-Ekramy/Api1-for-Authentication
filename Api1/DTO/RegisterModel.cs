using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace Api1.DTO
{
    public class RegisterModel
    {
        public string Name { get; set; }

        [EmailAddress(ErrorMessage ="Invalid Email Address")]
        public string Email { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
            ErrorMessage = "Passwords must be at least 6 characters long and contain at least one uppercase letter (A-Z), one lowercase letter (a-z), one number (0-9), and one special character (e.g., @$!%*?&).")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Address { get; set; }
    }
}
