namespace Library_Management_System.Models.VM.Book
{
    public class DisplayBookVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PublishYear { get; set; }
        public int AvailableCopies { get; set; }
        public string ISBN { get; set; }
        public string CategoryName { get; set; }
    }
}
