using Library_Management_System.Models.DTO.ApplicationUser;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UserController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        public async Task<IActionResult> Register(CreateUserDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var email = model.Email.Trim();
            var phone = model.Phone.Trim();

            bool emailTaken = await _userManager.FindByEmailAsync(email) != null;
            bool phoneTaken = await _userManager.Users.AnyAsync(u => u.PhoneNumber == phone);

            if (emailTaken || phoneTaken)
            {
                ModelState.AddModelError(string.Empty, "This email or phone number is already in use."); //not good for security 
                return View(model);   // pass the model back so the form keeps what the user typed
            }




            return View();
        }
    }
}
