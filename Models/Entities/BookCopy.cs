using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Management_System.Models.Entities
{
    public class BookCopy
    {
        public int Id { get; set; }
        public int CopyNumber { get; set; }
        [NotMapped]
        public string CopyCode => $"CPY:{CopyNumber}:D6";
        public bool IsAvailable { get; set; }
        public BookCopyStatus Status { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public List<Loan>? Loans { get; set; } // Navigation property for the loans associated with this book copy

    }

    public enum BookCopyStatus
    {
        Available,
        Borrowed,
        Reserved,
        Lost
    }
}
