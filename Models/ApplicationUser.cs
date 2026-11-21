using Microsoft.AspNetCore.Identity;

namespace Library_Management_System.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int MembershipNumber { get; set; }
        public DateTime CreatedA { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false; //soft delete property to indicate if the user is deleted or not

        public ICollection<Loan> Loans { get; set; } // Navigation property for the loans associated with the user
    }
}