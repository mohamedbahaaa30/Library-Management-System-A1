using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library_Management_System.Models.Configuration
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.Property(l => l.LoanDate).IsRequired();
            builder.Property(l => l.DueDate).IsRequired();
            builder.Property(l => l.ReturnDate);
            builder.Property(l => l.ApplicationUserId).IsRequired();
            builder.Property(l => l.BookCopyId).IsRequired();

            // Configure the relationship with Book
            builder.HasOne(l => l.BookCopy)
                   .WithMany(b => b.Loans)
                   .HasForeignKey(l => l.BookCopyId)
                   .OnDelete(DeleteBehavior.Cascade);
        }  
    }
}
