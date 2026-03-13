using System.ComponentModel.DataAnnotations;

namespace ECommerce_MVC.Models.VIEWMODELS
{
    public class RegisterVM
    {
        [Required]
        public string FullName { get; set; }
        [EmailAddress]
        [Required]
        [StringLength(100, ErrorMessage = "Email must be less than 100 characters.")]
        public string Email { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm Password does not match.")]
        public string ConfirmPassword { get; set; }
    }
}
