using System.ComponentModel.DataAnnotations;

namespace Api1.DTO
{
    public class ResetPasswordModel
    {

        public string Email { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
          ErrorMessage = "Passwords must be at least 6 characters long and contain at least one uppercase letter (A-Z), one lowercase letter (a-z), one number (0-9), and one special character (e.g., @$!%*?&).")]
        public string NewPassword { get; set; }
    }
}
