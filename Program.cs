using Library_Management_System.Models.Options;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add the database context to the services container
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<Context>()
    .AddDefaultTokenProviders();

//configure library policies 
builder.Services.Configure<LibraryPolicy>(builder.Configuration.GetSection("LibraryPolicy"));


builder.Services.ConfigureApplicationCookie(o =>
{
    // where to send people
    o.LoginPath = "/User/SignIn";
    o.LogoutPath = "/User/Logout";
    o.AccessDeniedPath = "/User/AccessDenied";

    // cookie identity and security
    o.Cookie.Name = "LibraryAuth";
    o.Cookie.HttpOnly = true;                                   // JavaScript cannot read the cookie
    o.Cookie.SecurePolicy = CookieSecurePolicy.Always;          // only sent over HTTPS
    o.Cookie.SameSite = SameSiteMode.Lax;                       // basic protection against cross-site requests

    // lifetime
    o.ExpireTimeSpan = TimeSpan.FromDays(7);                    // how long a persistent login lasts
    o.SlidingExpiration = true;                                 // each visit renews the timer
});

//register loan service 
builder.Services.AddScoped<ILoanService, LoanService>();

var app = builder.Build();

//seeding the roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Admin", "Member" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();   
app.UseAuthorization();    

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
