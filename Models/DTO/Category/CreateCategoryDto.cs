namespace Library_Management_System.Models.DTO.Category
{
    public class CreateCategoryDto
    {
        [Required: Required(ErrorMessage = "Category name is required.")]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required: Required(ErrorMessage = "Category description is required.")]
        [MaxLength(200)]
        public string Description { get; set; }

    }
}
