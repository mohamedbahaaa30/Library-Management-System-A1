using Library_Management_System.Models.VM.ApplicationUser;

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
        public IActionResult SignUp() => View(); 
        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterVM model)
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
                UserName = email,
                Email = email,
                FullName = model.Name,
                PhoneNumber = phone,
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
            return RedirectToAction(nameof(SignUp));
        }
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View("SignIn", model);

            //check for the email 
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");   
                return View(nameof(SignIn), model);
            }

            var result = await _signInManager.PasswordSignInAsync( user, model.Password, true, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "you entered worng information");
                return View(nameof(SignIn), model);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]                                                  
        [ValidateAntiForgeryToken]                                  
        public async Task<IActionResult> Logout()                   
        {                                                           
            await _signInManager.SignOutAsync();                    
            return RedirectToAction("SignIn");                      
        }
    }
}
