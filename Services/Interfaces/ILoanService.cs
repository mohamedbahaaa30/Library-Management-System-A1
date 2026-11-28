using Library_Management_System.Models.VM.Loan;
using Library_Management_System.Services.Common;

namespace Library_Management_System.Services.Interfaces
{
    public interface ILoanService 
    {
        Task<ServiceResult<LoanViewModel>> BorrowAsync(string userId, int CopyId);
        Task<ServiceResult<LoanViewModel>> ReturnAsync(int loanId);
        Task<ServiceResult<LoanViewModel>> RenewAsync(int loanId);
        Task<ServiceResult<List<LoanViewModel>>> GetAllAsync(string userId);
        Task<ServiceResult<List<LoanViewModel>>> GetActiveAsync(string userId);
    }
}
