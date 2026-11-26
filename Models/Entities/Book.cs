namespace Library_Management_System.Models.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PublishYear { get; set; }
        public int AvailableCopies { get; set; }
        public string? ISBN { get; set; } // International Standard Book Number = barcode  // old books before 1970 may not have ISBN 
        public bool? IsDeleted { get; set; } //soft delete flag

        // Navigation properties
        public List<Author>? Authors { get; set; } // Navigation property for the authors of this book
        public int CategoryId { get; set; } 
        public Category Category { get; set; } // Navigation property for the category of this book
        public List<BookCopy>? BookCopies { get; set; } // Navigation property for the copies of this book



    }
}
