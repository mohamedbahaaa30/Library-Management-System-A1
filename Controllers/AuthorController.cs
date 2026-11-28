namespace Library_Management_System.Controllers
{
    public class AuthorController : Controller
    {
        private readonly Context _context;
        public AuthorController(Context context)
        {
            this._context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var authors = await AuthorsAsDisplay().AsNoTracking().ToListAsync();
            return authors.Any() ? View(authors) : View("Empty");
        }
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var author = await AuthorsAsDisplay().FirstOrDefaultAsync(u => u.Id==id);
            return author == null ? NotFound() : View(author);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreateAuthorVM model)
        {
            if (!ModelState.IsValid)
                return View(nameof(Create), model);

            var name = model.Name.Trim();
            if (await _context.Authors.AnyAsync(A => A.Name == name))
            {
                ModelState.AddModelError("name","author with this name is already exist");
                return View(nameof(Create), model);
            }
            Author author = new Author()
            {
                Name = name,
                Bio = model.Bio
            };
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);
            if (author == null)
            {
                return NotFound();
            }
            return View( new EditAuthorVM {Id = author.Id , Name = author.Name , Bio = author.Bio});
        }
        [HttpPost]
        public async Task<IActionResult> Update(EditAuthorVM model)
        {
            if (!ModelState.IsValid)
                return View(nameof(Edit),model);

            var exist = await _context.Authors.FindAsync(model.Id);
            if(exist == null) return NotFound();
            
            var name = model.Name.Trim();
            var takenName = await _context.Authors.AnyAsync(A => A.Name == name && A.Id != model.Id);
            if (takenName)
            {
                ModelState.AddModelError("Name", "Author with this name alread exist");
                return View(nameof(Edit), model);
            }
            exist.Name = name;
            exist.Bio = model.Bio;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetById) , new { id = exist.Id });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            var item = await _context.Authors.FindAsync(Id);
            if (item == null)
                return NotFound();
             _context.Authors.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private IQueryable<DisplayAuthorVM> AuthorsAsDisplay()
        {
            return _context.Authors.AsNoTracking()
                .Select(a => new DisplayAuthorVM
                {
                    Id = a.Id,
                    Name = a.Name,
                    Bio = a.Bio,
                    BookCount = a.Books!.Count,
                    BookTitles = a.Books.Select(b => b.Title).ToList()
                });
        }
    }
}
