namespace Library_Management_System.Models.Options
{
    public class LibraryPolicy
    {
        [Range(0.01, 1000)]
        public decimal FinePerDay { get; set; }
        public int LoanDurationDays { get; set; }
        public int MaxActiveLoans { get; set; }
        public int RenewBorrow { get; set; }
    
    }
    
}
