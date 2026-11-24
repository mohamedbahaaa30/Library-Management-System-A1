namespace Library_Management_System.Models.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }

        public List<Book>? Books { get; set; } // Navigation property for the books written by the author
    }
}
