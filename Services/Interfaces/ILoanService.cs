using Library_Management_System.Services.Common;

namespace Library_Management_System.Services.Interfaces
{
    public interface ILoanService 
    {
        Task<ServiceResult<Loan>> BorrowAsync(string userId, int CopyId);
        Task<ServiceResult<Loan>> ReturnAsync(int loanId);
        Task<ServiceResult<Loan>> RenewAsync(int loanId);
        Task<ServiceResult<Loan>> GetAllAsync(string userId);
        Task<ServiceResult<Loan>> GetActiveAsync(string userId);
    }
}
