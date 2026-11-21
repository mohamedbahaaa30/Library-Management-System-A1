namespace Library_Management_System.Models.VM.Book
{
    public class EditBookVM
    {
        [Required]
        public string Title { get; set; }
        [Required]
        [MaxLength(100)]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public int PublishYear { get; set; }
        [Required]
        public int AvailableCopies { get; set; }
        public int? ISBN { get; set; }
        public int CategoryId { get; set; }
        public List<int> AuthorIds { get; set; } = new List<int>();

        //to populate the dropdown lists in the view
        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Authors { get; set; } = new List<SelectListItem>();
    }
}
