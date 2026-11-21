namespace Library_Management_System.Models.DTO.Author
{
    public class CreateAuthorDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Bio { get; set; }
    }
}
