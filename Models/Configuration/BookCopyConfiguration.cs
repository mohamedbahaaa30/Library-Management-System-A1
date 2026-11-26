namespace Library_Management_System.Models.Configuration
{
    public class BookCopyConfiguration : IEntityTypeConfiguration<BookCopy>
    {
        public void Configure(EntityTypeBuilder<BookCopy> builder)
        {
            //store the enum in db as string
            builder.Property(b => b.Status)
                .HasConversion<string>()
                .HasMaxLength(30);


            //build the counter to book copy number
            builder.Property(b => b.CopyNumber)
                .HasDefaultValueSql("NEXT VALUE FOR CopyNumber");

            builder.HasIndex(b => b.CopyNumber)
                .IsUnique();
        }
    }
}
