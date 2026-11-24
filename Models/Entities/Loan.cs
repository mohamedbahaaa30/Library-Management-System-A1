namespace Library_Management_System.Models.Entities
{
    public class Loan
    {
        public int Id { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        // Navigation property for the user who borrowed the book copy
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } // Navigation property for the user who borrowed the book copy

        public int BookId { get; set; }
        public Book Book { get; set; }

        public List<Fine> Fines { get; set; } // Navigation property for the fines associated with this loan 
        // one load can have many fines : borrowed book can have many fines if the user didn't return it on time and keep it for a long tim
    }
}
