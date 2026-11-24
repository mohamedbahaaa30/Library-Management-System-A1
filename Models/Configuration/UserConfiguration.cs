using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library_Management_System.Models.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.MembershipNumber)
                 .IsRequired()
                 .HasMaxLength(50)
                 .HasColumnType("nvarchar(50)");
            builder.Property(u => u.CreatedAt)
                .IsRequired();

            builder.Property(u => u.MembershipNumber)
                .HasDefaultValueSql("NEXT VALUE FOR MembershipNumbers");   // property must be int

            builder.HasIndex(u => u.MembershipNumber).IsUnique();

            builder.HasIndex(u => u.PhoneNumber).IsUnique();
            builder.HasIndex(u => u.Email).IsUnique();

            // Configure the relationship with Loan
            builder.HasMany(u => u.Loans)
                .WithOne(l => l.ApplicationUser)
                .HasForeignKey(l => l.ApplicationUserId);

                
        }
    }
}
