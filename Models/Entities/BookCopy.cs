namespace Library_Management_System.Models.Entities
{
    public class BookCopy
    {
        public int Id { get; set; }
        public int Barcode { get; set; }
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
