using Library_Management_System.Models.VM.Book;
using Library_Management_System.Models.VM.Category;

namespace Library_Management_System.Controllers
{
    public class BookController : Controller
    {
        private readonly Context _context;
        public BookController(Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var results = BooksAsDisplay().ToList();
            return results.Count() > 0 ? View(results) : View("Empty");
        }
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await BooksAsDisplay().FirstOrDefaultAsync(c => c.Id == id);
            return book == null ? NotFound() : View(book);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateBookVM();
            await FillDropdownsAsync(model);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreateBookVM model)
        {
            if (!ModelState.IsValid)
            {
                await FillDropdownsAsync(model);
                return View(nameof(Create), model);
            }
            var title = model.Title.Trim();

            if (await _context.Books.AnyAsync(b => b.Title == title))
                ModelState.AddModelError(nameof(model.Title), "A book with this title already exists.");

            if (!await _context.Categories.AnyAsync(c => c.Id == model.CategoryId))
                ModelState.AddModelError(nameof(model.CategoryId), "Please select a valid category.");

            //the vm contain list of authors ids -> so bring their objets 
            var authorIds = model.AuthorIds.Distinct().ToList(); //unique id list 
            var authors = await _context.Authors
                    .Where(a => authorIds.Contains(a.Id))
                    .ToListAsync();
            if (authors.Count != authorIds.Count)
                ModelState.AddModelError(nameof(model.AuthorIds), "One or more selected authors do not exist.");

            if (!ModelState.IsValid) //check again 
            {
                await FillDropdownsAsync(model);
                return View("Create", model);
            }

            Book book = new Book
            {
                Title = title,
                Description = model.Description,
                PublishYear = model.PublishYear,
                AvailableCopies = model.AvailableCopies,
                ISBN = model.ISBN,
                CategoryId = model.CategoryId,
                Authors = authors
            };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _context.Books.AsNoTracking()
                    .Include(b => b.Authors)
                    .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound();

            var model = new EditBookVM
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                PublishYear = book.PublishYear,
                AvailableCopies = book.AvailableCopies,
                ISBN = book.ISBN,
                CategoryId = book.CategoryId,
                AuthorIds = book.Authors.Select(a => a.Id).ToList()
            };

            await FillDropdownsAsync(model);
            return View(model);
        }
        [HttpPost()]
        public async Task<IActionResult> Update(EditBookVM model)
        {
            if (!ModelState.IsValid)
            {
                await FillDropdownsAsync(model); // fill the dropdown list again
                return View(nameof(Edit),model);
            }
            var existingBook = await _context.Books.Include(b => b.Authors).FirstOrDefaultAsync(b => b.Id == model.Id);
            if (existingBook == null) return NotFound();

            var title = model.Title.Trim();
            if (await _context.Books.AnyAsync(b => b.Title == title && b.Id != model.Id))
                ModelState.AddModelError(nameof(model.Title), "A book with this title already exists.");

            if (!await _context.Categories.AnyAsync(c => c.Id == model.CategoryId))
                ModelState.AddModelError(nameof(model.CategoryId), "Please select a valid category.");

            //the vm contain list of authors ids -> so bring their objets 
            var authorIds = model.AuthorIds.Distinct().ToList(); //unique id list 
            var authors = await _context.Authors
                    .Where(a => authorIds.Contains(a.Id))
                    .ToListAsync();
            if (authors.Count != authorIds.Count)
                ModelState.AddModelError(nameof(model.AuthorIds), "One or more selected authors do not exist.");

            if (!ModelState.IsValid)
            {
                await FillDropdownsAsync(model);
                return View("Edit", model);
            }

            existingBook.Title = title;
            existingBook.Description = model.Description;
            existingBook.PublishYear = model.PublishYear;
            existingBook.AvailableCopies = model.AvailableCopies;
            existingBook.ISBN = model.ISBN;
            existingBook.CategoryId = model.CategoryId;
            existingBook.Authors = authors;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetById), new { id = existingBook.Id });
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)  return NotFound();
            _context.Books.Remove(book);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // fill the dropdown list
        public async Task FillDropdownsAsync(CreateBookVM model)
        {
            model.Categories = await _context.Categories
                .OrderBy(c => c.Name).Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToListAsync();

            model.Authors = await _context.Authors
                .OrderBy(a => a.Name).Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name }).ToListAsync();
        }
        public async Task FillDropdownsAsync(EditBookVM model)
        {
            model.Categories = await _context.Categories
                .OrderBy(c => c.Name).Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToListAsync();

            model.Authors = await _context.Authors
                .OrderBy(a => a.Name).Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name }).ToListAsync();
        }

        // display vm
        private IQueryable<DisplayBookVM> BooksAsDisplay() =>
            _context.Books
                .AsNoTracking()
                .Select(b => new DisplayBookVM
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    PublishYear = b.PublishYear,
                    AvailableCopies = b.AvailableCopies,
                    ISBN = b.ISBN,
                    CategoryName = b.Category.Name,
                    AuthorNames = b.Authors.Select(a => a.Name).ToList()
                });

    }
}
