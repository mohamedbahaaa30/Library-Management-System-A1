namespace Library_Management_System.Models.Data
{
    public class Context : IdentityDbContext<ApplicationUser>
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<BookCopy> BookCopies { get; set; }
        public DbSet<Fine> Fines { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasSequence<int>("MembershipNumbers")
                .StartsAt(1000)
                .IncrementsBy(1);

            builder.HasSequence<int>("CopyNumber")
                .StartsAt(5000)
                .IncrementsBy(1);

            builder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly);   // loads UserConfiguration
        }

    }
}
