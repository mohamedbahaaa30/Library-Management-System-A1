namespace Library_Management_System.Models.VM.Author
{
    public class CreateAuthorVM
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bio is required")]
        [MaxLength(1000)]
        public string Bio { get; set; }
    }
}
