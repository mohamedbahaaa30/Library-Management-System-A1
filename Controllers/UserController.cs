using Library_Management_System.Models.DTO.ApplicationUser;
using Library_Management_System.Models.Entities;
using Library_Management_System.Services.Implemetation;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        public UserController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager  , ITokenService tokenService)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._tokenService = tokenService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp()
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
            //map
            var newUser = new ApplicationUser
            {
                UserName = model.Name,
                PhoneNumber = model.Phone,
                Email = model.Email,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            };
            //create
            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError(string.Empty, err.Description);
                return View(model);
            }
            await _userManager.AddToRoleAsync(newUser, "Member");   // optional; the role must already exist
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
                return View("login",model);

            //check for the email 
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user == null)
            {
                ModelState.AddModelError(string.Empty, "there is no accout for this Email");
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync( user, model.Password, true, lockoutOnFailure: true);

            //token 
            var token = await _tokenService.CreateToken(user);
            return Ok(new { token });
        }
    }
}
