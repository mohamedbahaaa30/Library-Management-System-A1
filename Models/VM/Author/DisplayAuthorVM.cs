namespace Library_Management_System.Models.VM.Author
{
    public class DisplayAuthorVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public int BookCount { get; set; }
        public List<string> BookTitles { get; set; } = new List<string>();
    }
}
