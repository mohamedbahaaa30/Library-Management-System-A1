using Library_Management_System.Models.Options;
using Library_Management_System.Models.VM.Loan;
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

        public async Task<ServiceResult<LoanViewModel>> BorrowAsync(string userId, int BookId)
        {
            // validate that the book exists
            var book = await _context.Books.FindAsync(BookId);
            if (book == null)
                return ServiceResult<LoanViewModel>.Fail("That book does not exist");

            // check the user's active loans against the policy limit 3

            var activeLoans = await _context.Loans.CountAsync(l => l.ApplicationUserId == userId && l.ReturnDate == null);
            if (activeLoans >= _libraryPolicy.MaxActiveLoans)
                return ServiceResult<LoanViewModel>.Fail($"You reached the maximum of {_libraryPolicy.MaxActiveLoans} active loans");

            // make sure the user doesn't already hold a copy of this book
            var alreadyBorrowed = await _context.Loans.AnyAsync(l =>l.ApplicationUserId == userId &&l.ReturnDate == null &&l.BookCopy.BookId == BookId);
            if (alreadyBorrowed)
                return ServiceResult<LoanViewModel>.Fail("You already have a copy of this book");

            // pick the first available copy (this is the real source of truth)
            var copy = await _context.BookCopies.Where(c => c.BookId == BookId && c.IsAvailable).OrderBy(c => c.Id).FirstOrDefaultAsync();
            if (copy == null)
                return ServiceResult<LoanViewModel>.Fail("There are no available copies");

            var now = DateTime.UtcNow;

            copy.IsAvailable = false;
            copy.Status = BookCopyStatus.Borrowed;
            book.AvailableCopies--;

            var newLoan = new Loan
            {
                ApplicationUserId = userId,
                BookCopyId = copy.Id,
                CopyCode = copy.CopyCode,
                LoanDate = now,
                DueDate = now.AddDays(_libraryPolicy.LoanDurationDays)
            };

            _context.Loans.Add(newLoan);
            await _context.SaveChangesAsync();

            return ServiceResult<LoanViewModel>.Ok(new LoanViewModel
            {
                LoanId = newLoan.Id,
                BookTitle = book.Title,
                CopyCode = newLoan.CopyCode,
                LoanDate = newLoan.LoanDate,
                DueDate = newLoan.DueDate,
                ReturnDate = null,
                IsOverdue = false
            });
        }
        public async Task<ServiceResult<LoanViewModel>> RenewAsync(int loanId )
        {
            // validate the loan and make sure it belongs to this user
            var loan = await _context.Loans
                .Include(l => l.BookCopy).ThenInclude(bc => bc.Book)
                .FirstOrDefaultAsync(l => l.Id == loanId);

            if (loan == null)
                return ServiceResult<LoanViewModel>.Fail("Loan not found");

            if (loan.ReturnDate != null)
                return ServiceResult<LoanViewModel>.Fail("This loan was already returned");

            if (loan.DueDate < DateTime.UtcNow)
                return ServiceResult<LoanViewModel>.Fail("Overdue loans cannot be renewed");

            // extend from the current due date, not from today
            loan.DueDate = loan.DueDate.AddDays(_libraryPolicy.RenewBorrow);
            await _context.SaveChangesAsync();

            return ServiceResult<LoanViewModel>.Ok(new LoanViewModel
            {
                LoanId = loan.Id,
                BookTitle = loan.BookCopy.Book.Title,
                CopyCode = loan.CopyCode,
                LoanDate = loan.LoanDate,
                DueDate = loan.DueDate,
                ReturnDate = loan.ReturnDate,
                IsOverdue = false
            });
        }
        public async Task<ServiceResult<LoanViewModel>> ReturnAsync(int loanId)
        {
            //validate the loan 
            var loanExist = await _context.Loans.Include(l => l.BookCopy).ThenInclude(bc => bc.Book).FirstOrDefaultAsync(l => l.Id == loanId);
            if (loanExist == null)
                return ServiceResult<LoanViewModel>.Fail("you entered wrong info about your last loan");
            if (loanExist.ReturnDate != null)
                return ServiceResult<LoanViewModel>.Fail("This loan was already returned");

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
            return ServiceResult<LoanViewModel>.Ok(new LoanViewModel
            {
                BookTitle = loanExist.BookCopy.Book.Title,
                CopyCode = loanExist.CopyCode,
                LoanDate = loanExist.LoanDate,
                DueDate = (DateTime)loanExist.ReturnDate,
                IsOverdue = loanExist.ReturnDate > loanExist.DueDate
            });
        }
        public async Task<ServiceResult<List<LoanViewModel>>> GetAllAsync(string id)
        {
            var result = await _context.Loans.AsNoTracking().Where(l => l.ApplicationUserId == id)
                                .OrderBy(l => l.DueDate).Select(l => new LoanViewModel
                                {
                                    LoanId = l.Id,
                                    BookTitle = l.BookCopy.Book.Title,
                                    CopyCode = l.CopyCode,
                                    LoanDate = l.LoanDate,
                                    DueDate = l.DueDate,
                                    ReturnDate = l.ReturnDate
                                })
                                .ToListAsync();

            return ServiceResult<List<LoanViewModel>>.Ok(result);
        }
        public async Task<ServiceResult<List<LoanViewModel>>> GetActiveAsync(string id)
        {
            var result = await _context.Loans.AsNoTracking().Where(l => l.ApplicationUserId == id && l.ReturnDate == null)
                                .OrderBy(l => l.DueDate).Select(l => new LoanViewModel
                                {
                                    LoanId = l.Id,
                                    BookTitle = l.BookCopy.Book.Title,
                                    CopyCode = l.CopyCode,
                                    LoanDate = l.LoanDate,
                                    DueDate = l.DueDate,
                                    ReturnDate = l.ReturnDate
                                })
                                .ToListAsync();

            return ServiceResult<List<LoanViewModel>>.Ok(result);
        }
    }
}
