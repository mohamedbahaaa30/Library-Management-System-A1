namespace Library_Management_System.Models.DTO.ApplicationUser
{
    public class LoginDto
    {

        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string? Email { get; set; }

        [Required(ErrorMessage ="please enter your password")]
        [DataType(dataType:DataType.Password)]
        public string Password { get; set; }

    }
}
