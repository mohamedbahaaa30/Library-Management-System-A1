namespace Library_Management_System.Models.DTO.ApplicationUser
{
    public class EditUserDto
    {
        [Required(ErrorMessage = "Name field is Required")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "phone field is Required")]
        [DataType(dataType: DataType.PhoneNumber)]
        public int phone { get; set; }

        [Required(ErrorMessage = "Email field is Required")]
        [DataType(dataType: DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
