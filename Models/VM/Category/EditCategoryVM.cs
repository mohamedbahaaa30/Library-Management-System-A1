namespace Library_Management_System.Models.VM.Category
{
    public class EditCategoryVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Category name is required.")]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Category description is required.")]
        [MaxLength(200)]
        public string Description { get; set; }
    }
}
