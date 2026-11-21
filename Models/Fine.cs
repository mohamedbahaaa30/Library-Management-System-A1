namespace Library_Management_System.Models
{
    public class Fine
    {
        public int Id { get; set; }
        public int LoanId { get; set; }
        public Loan Loan { get; set; }

        public DateTime FineDate { get; set; }
        public decimal Amount { get; set; }
    }
}
