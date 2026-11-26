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
            var authors = await _context.Authors.AsNoTracking().ToListAsync();
            return authors.Any() ? View(authors) : View("Empty");
        }
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _context.Authors.FindAsync(id);
            return result == null ? NotFound() : View(result);
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
                return View(model);

            if (await _context.Authors.AnyAsync(A => A.Name == model.Name))
            {
                ModelState.AddModelError("name","author with this name is already exist");
                return View(model);
            }
            Author author = new Author()
            {
                Name = model.Name,
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
            EditAuthorVM editAuthorVM = new EditAuthorVM
            {
                Id = author.Id,
                Name = author.Name,
                Bio = author.Bio
            };
            return View( new EditAuthorVM {Id = author.Id , Name = author.Name , Bio = author.Bio});
        }
        [HttpPost]
        public async Task<IActionResult> Update(EditAuthorVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var exist = await _context.Authors.FindAsync(model.Id);
            if(exist == null) return NotFound();
            

            var name = model.Name.Trim();
            var takenName = await _context.Authors.AnyAsync(A => A.Name == model.Name && A.Id != model.Id);
            if (takenName)
            {
                ModelState.AddModelError("Name", "Author with this name alread exist");
                return View(model);
            }
            exist.Name = model.Name;
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
    }
}
