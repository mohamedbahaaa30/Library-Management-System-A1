namespace Library_Management_System.Models.VM.Category
{
    public class DisplayCategoryVM
    {
        public int Id { get; set; }

        [Display(Name = "Category name")]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        // Handy for the list page, filled with a Count() in your query projection
        [Display(Name = "Books")]
        public int BookCount { get; set; }
    }
}
