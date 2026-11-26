namespace Library_Management_System.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UserController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager )
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(CreateUserDto model)
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
                //username must be unique
                UserName = model.Email,
                Email = model.Email,
                FullName = model.Name,
                PhoneNumber = model.Phone,
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
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(LoginDto model)
        {
            if (!ModelState.IsValid)
                return View("SignIn", model);

            //check for the email 
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "there is no accout for this Email");
                return View("SignIn", model);
            }

            var result = await _signInManager.PasswordSignInAsync( user, model.Password, true, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "you entered worng information");
                return View("SignIn", model);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
