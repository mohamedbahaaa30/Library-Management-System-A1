namespace Library_Management_System.Models.VM.ApplicationUser
{
    public class EditUserVM
    {
        [Required]
        public string Id { get; set; } = string.Empty;

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
    }
}
