namespace Library_Management_System.Models.DTO.ApplicationUser
{
    public class CreateUserDto
    {
        [Required(ErrorMessage ="Name field is Required")]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required(ErrorMessage = "phone field is Required")]
        [DataType(dataType:DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Email field is Required")]
        [DataType(dataType: DataType.EmailAddress)]
        public string Email { get; set; }
        [MaxLength(50)]
        [Required(ErrorMessage = "Password field is Required")]
        [DataType(dataType: DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Password confirmation field is Required")]
        [DataType(dataType: DataType.Password)]
        [Compare("password")]
        public string ConfirmPassword { get; set; }
    }
}
