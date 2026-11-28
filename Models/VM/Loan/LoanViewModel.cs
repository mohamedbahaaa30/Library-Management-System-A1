namespace Library_Management_System.Models.VM.Loan
{
    public class LoanViewModel
    {
        public int LoanId { get; set; }
        public string BookTitle { get; set; }
        public string CopyCode { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsOverdue { get; set; }
    }
}
