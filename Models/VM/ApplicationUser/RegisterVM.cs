namespace Library_Management_System.Models.VM.ApplicationUser
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        [Display(Name = "Full name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be between {2} and {1} characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password confirmation is required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
