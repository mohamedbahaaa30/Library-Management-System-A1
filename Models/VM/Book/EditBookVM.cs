using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Library_Management_System.Models.VM.Book
{
    public class EditBookVM
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Publish year is required")]
        [Range(1000, 2100, ErrorMessage = "Please enter a valid year")]
        [Display(Name = "Publish year")]
        public int PublishYear { get; set; }

        [Required(ErrorMessage = "Available copies is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Copies cannot be negative")]
        [Display(Name = "Available copies")]
        public int AvailableCopies { get; set; }

        [StringLength(17)]
        public string? ISBN { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a category")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [MinLength(1, ErrorMessage = "Select at least one author")]
        [Display(Name = "Authors")]
        public List<int> AuthorIds { get; set; } = new();

        [ValidateNever]
        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();

        [ValidateNever]
        public IEnumerable<SelectListItem> Authors { get; set; } = new List<SelectListItem>();
    }
}
