using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Management_System.Models.Entities
{
    public class Fine
    {
        public int Id { get; set; }

        public int LoanId { get; set; }
        public Loan Loan { get; set; }

        public DateTime FineDate { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }
    }
}
