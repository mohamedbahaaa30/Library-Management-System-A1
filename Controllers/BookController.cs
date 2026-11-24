using Library_Management_System.Models.DTO.Book;
using Library_Management_System.Models.Entities;
using Library_Management_System.Models.VM.Book;

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
            var results = _context.Books.Include(b => b.Category).ToList();
            return results.Count() > 0 ? View(results) : View("Empty");
        }
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.Books.AsNoTracking().Include(b => b.Category).FirstOrDefaultAsync(b => b.Id == id);
            return item != null ? View(item) : NotFound();
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateBookDto();
            await FillDropdownsAsync(model);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreateBookDto model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var Exist = _context.Books.FirstOrDefault(b => b.Title == model.Title);
            if(Exist != null)
            {
                ModelState.AddModelError("Title", "A book with this title already exists.");
                return View(model);
            }

            Book book = new Book
            {
                Title = model.Title,
                Description = model.Description,
                PublishYear = model.PublishYear,
                AvailableCopies = model.AvailableCopies,
                ISBN = model.ISBN,
                CategoryId = model.CategoryId
            };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Populate the dropdown lists for authors and categories
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            DisplayBookVM displayBookVM = new DisplayBookVM
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                PublishYear = book.PublishYear,
                AvailableCopies = book.AvailableCopies,
                ISBN = book.ISBN.ToString(),
                CategoryName = _context.Categories.FirstOrDefault(c => c.Id == book.CategoryId)?.Name
            };

            return View(displayBookVM);
        }
        [HttpPut()]
        public async Task<IActionResult> Update( UpdateBookDto model)
        {
            if (!ModelState.IsValid) //validate the data 
            {
                return View(model);
            }
            var existingBook = await _context.Books.FindAsync(model.Id); // check if the book exists in the database 
            if (existingBook == null)
                return NotFound();
            
            var TitleTaken = await _context.Books.AnyAsync(b => b.Title == model.Title && b.Id != model.Id); // Check if a book with the same title already exists
            if (TitleTaken)
            {
                ModelState.AddModelError("Title", "A book with this title already exists.");
                return View(model);
            }

            existingBook.Title = model.Title;
            existingBook.Description = model.Description;
            existingBook.PublishYear = model.PublishYear;
            existingBook.AvailableCopies = model.AvailableCopies;
            existingBook.ISBN = model.ISBN;
            existingBook.CategoryId = model.CategoryId;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            _context.Books.Remove(book);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task FillDropdownsAsync(CreateBookDto model)
        {
            model.Categories = await _context.Categories
                .OrderBy(c => c.Name).Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToListAsync();

            model.Authors = await _context.Authors
                .OrderBy(a => a.Name).Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name }).ToListAsync();

        }

    }
}
