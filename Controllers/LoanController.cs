using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Library_Management_System.Controllers
{
    public class LoanController : Controller
    {
        private readonly ILoanService _loanService ;
        public LoanController(ILoanService loanService)
        {
            this._loanService = loanService;
        }
        //get current user id
        private string CurrentUserId =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
            ?? throw new UnauthorizedAccessException ();

        public async Task<IActionResult> GetLAll()
            => View(_loanService.GetAllAsync(CurrentUserId));

        public async Task<IActionResult> GetActive()
            => View(_loanService.GetActiveAsync(CurrentUserId));

        public async Task<IActionResult> Borrow(int bookId)
            => View(_loanService.BorrowAsync(CurrentUserId, bookId));

        public async Task<IActionResult> Renew(int id)
            => View(_loanService.RenewAsync(id));

        public async Task<IActionResult> Return(int id)
            => View(_loanService.ReturnAsync(id));

        public IActionResult Index()
        {
            return View();
        }
    }
}
