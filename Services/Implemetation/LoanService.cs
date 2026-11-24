using Library_Management_System.Models.Options;
using Library_Management_System.Services.Common;

namespace Library_Management_System.Services.Implemetation
{
    public class LoanService : ILoanService
    {
        private readonly Context _context;
        private readonly LibraryPolicy _libraryPolicy;
        public LoanService(Context context, IOptions<LibraryPolicy> policy)
        {
            this._context = context;
            this._libraryPolicy = policy.Value; //IOptions<T> is a wrapper. The actual object is inside its .Value
        }
        public async Task<ServiceResult<Loan>> BorrowAsync(string userId, int BookId)
        {
            //validate if that book already exist
            var book = await _context.Books.FindAsync(BookId);
            if(book == null)
                return ServiceResult<Loan>.Fail("that book is not available");
               
            //check if there is available copies
            if (book.AvailableCopies == 0)
                return ServiceResult<Loan>.Fail("there is no available copies");

            //check if user has more than 3 loans
            var activeLoans = await _context.Loans.CountAsync(l => l.ApplicationUserId == userId && l.ReturnDate == null);
            if (activeLoans >= _libraryPolicy.MaxActiveLoans)
                return ServiceResult<Loan>.Fail("there is no available copies");

            //validate that this user does not have a copy book 
            var copies = await _context.BookCopies.Where(b => b.BookId == BookId).Select(b=>b.Id).ToListAsync();
            var alreadyBorrowed =  await _context.Loans.AnyAsync(l => l.ApplicationUserId == userId && l.ReturnDate == null && copies.Contains(l.BookCopyId));
            if (alreadyBorrowed)
                return ServiceResult<Loan>.Fail("this user hase a copy for that book ");
            var copy = await _context.BookCopies.Where(c => c.BookId == BookId && c.IsAvailable)
                        .OrderBy(c => c.Id)
                        .FirstOrDefaultAsync();

            copy.IsAvailable = false;
            copy.Status = BookCopyStatus.Borrowed;

            var newLoan = new Loan
            {
                ApplicationUserId = userId,
                BookCopyId = copy.Id,
                CopyCode = copy.CopyCode,
                LoanDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(_libraryPolicy.LoanDurationDays)
            };
            //decrease the counter of available copies
            book.AvailableCopies--;

            await _context.AddAsync(newLoan);
            await _context.SaveChangesAsync();
            return ServiceResult<Loan>.Ok(newLoan);
        }
        public async Task<ServiceResult<Loan>> RenewAsync(int loanId)
        {
            //validate the loan 
            var loanExist = await _context.Loans.FindAsync(loanId);
            if (loanExist == null)
                return ServiceResult<Loan>.Fail("you entered wrong info about your last loan");

            loanExist.DueDate = DateTime.UtcNow.AddDays(_libraryPolicy.RenewBorrow);
            await _context.SaveChangesAsync();

            return ServiceResult<Loan>.Ok(loanExist);
        }
        public async Task<ServiceResult<Loan>> ReturnAsync(int loanId)
        {
            //validate the loan 
            var loanExist = await _context.Loans.Include(l => l.BookCopy).ThenInclude(bc => bc.Book).FirstOrDefaultAsync(l => l.Id == loanId);
            if (loanExist == null)
                return ServiceResult<Loan>.Fail("you entered wrong info about your last loan");

            //check if over date 
            int daysLate = (DateTime.UtcNow - loanExist.DueDate).Days;
            if (daysLate > 14 )
            {
                // user must pay for its latency
                var fine = daysLate * _libraryPolicy.FinePerDay;
            }
            loanExist.ReturnDate = DateTime.UtcNow;

            loanExist.BookCopy.Book.AvailableCopies++;
            loanExist.BookCopy.IsAvailable = true;
            loanExist.BookCopy.Status = BookCopyStatus.Available;
            await _context.SaveChangesAsync();
            return ServiceResult<Loan>.Ok(loanExist);
        }
        public async Task<ServiceResult<Loan>> GetAllAsync(string id)
        {
            var result = await _context.Loans.Include(l => l.BookCopy).FirstOrDefaultAsync(l => l.ApplicationUserId == id);
            return ServiceResult<Loan>.Ok(result);
        }
        public async Task<ServiceResult<Loan>> GetActiveAsync(string id)
        {
            var result = await _context.Loans.Include(l => l.BookCopy).FirstOrDefaultAsync(l => l.ApplicationUserId == id && l.ReturnDate == null);
            return ServiceResult<Loan>.Ok(result);
        }
    }
}
