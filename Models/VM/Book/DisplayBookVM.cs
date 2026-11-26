namespace Library_Management_System.Models.VM.Book
{
    public class DisplayBookVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Publish year")]
        public int PublishYear { get; set; }

        [Display(Name = "Available copies")]
        public int AvailableCopies { get; set; }

        public string? ISBN { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; } = string.Empty;

        [Display(Name = "Authors")]
        public List<string> AuthorNames { get; set; } = new();
    }
}
