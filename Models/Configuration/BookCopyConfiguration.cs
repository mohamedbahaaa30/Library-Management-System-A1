namespace Library_Management_System.Models.Configuration
{
    public class BookCopyConfiguration : IEntityTypeConfiguration<BookCopy>
    {
        public void Configure(EntityTypeBuilder<BookCopy> builder)
        {
            builder.Property(b => b.CopyNumber)
                .HasDefaultValueSql("NEXT VALUE FOR CopyNumber");

            builder.HasIndex(b => b.CopyNumber)
                .IsUnique();
        }
    }
}
