namespace Library_Management_System.Models.VM.Category
{
    public class CreateCategoryVM
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(50)]
        [Display(Name = "Category name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category description is required")]
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;
    }
}
