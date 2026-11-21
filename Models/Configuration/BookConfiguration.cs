using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library_Management_System.Models.Configuration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Property(b => b.Title).IsRequired().HasMaxLength(200);
            builder.Property(b => b.ISBN).IsRequired().HasMaxLength(13);
            builder.Property(b => b.PublishYear).IsRequired();
            builder.Property(b => b.AvailableCopies).IsRequired();

            builder.HasIndex(b => b.ISBN).IsUnique();
            builder.HasIndex(b => b.Title).IsUnique();

            // Configure the relationship with Category
            builder.HasOne(b => b.Category)
                   .WithMany(c => c.Books)
                   .HasForeignKey(b => b.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
