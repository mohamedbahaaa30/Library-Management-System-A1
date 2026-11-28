using Microsoft.AspNetCore.Authorization;
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
            => View(await _loanService.GetAllAsync(CurrentUserId));
        public async Task<IActionResult> GetActive()
            => View(await _loanService.GetActiveAsync(CurrentUserId));
        public async Task<IActionResult> Borrow(int bookId)
        {
            var result = await _loanService.BorrowAsync(CurrentUserId, bookId);
            if (result.Success) TempData["Success"] = "Book borrowed.";
            else TempData["Error"] = result.Error;
            return RedirectToAction(nameof(GetActive));
        }
        public async Task<IActionResult> Renew(int loanId)
        {
            var result = await _loanService.RenewAsync(loanId);
            if (result.Success) TempData["Success"] = "Loan renewed.";
            else TempData["Error"] = result.Error;
            return RedirectToAction(nameof(GetActive));
        }
        public async Task<IActionResult> Return(int loanId)
        {
            var result = await _loanService.ReturnAsync(loanId);
            if (result.Success) TempData["Success"] = "Book returned.";
            else TempData["Error"] = result.Error;
            return RedirectToAction(nameof(GetActive));
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
