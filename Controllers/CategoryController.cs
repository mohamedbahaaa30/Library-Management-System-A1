using Library_Management_System.Models.VM.Category;
namespace Library_Management_System.Controllers
{
    public class CategoryController : Controller
    {
        private readonly Context _context;
        public CategoryController(Context context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await CategoriesAsDisplay().ToListAsync();
            return categories.Any() ? View(categories) : View("Empty");
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await CategoriesAsDisplay().FirstOrDefaultAsync(c => c.Id == id);
            return category == null ? NotFound() : View(category);
        }
        [HttpGet]
        public IActionResult Create() => View();
        
        [HttpPost]
        public async Task<IActionResult> Add(CreateCategoryVM model)
        {
            if (!ModelState.IsValid)  return View(nameof(Create),model);
            
            var Exist = _context.Categories.FirstOrDefault(c => c.Name == model.Name);
            if (Exist != null)
            {
                ModelState.AddModelError("Name", "A category with this name already exists.");
                return View(nameof(Create), model);
            }
            Category category = new Category
            {
                Name = model.Name,
                Description = model.Description
            };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();

            EditCategoryVM editCategoryVM = new EditCategoryVM
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
            return View(editCategoryVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(EditCategoryVM model)
        {
            if (!ModelState.IsValid) return View(nameof(Edit),model);
            
            var Exist = await _context.Categories.FindAsync(model.Id);
            if (Exist == null) return NotFound();

            var takenName = await _context.Categories.AnyAsync(c => c.Name == model.Name && c.Id != model.Id );
            if (takenName)
            {
                ModelState.AddModelError("Name", "A category with this name already exists.");
                return View(nameof(Edit), model);
            }
            Exist.Name = model.Name;
            Exist.Description = model.Description;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetById), new {id =Exist.Id});
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();
            
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        //display as vm
        private IQueryable<DisplayCategoryVM> CategoriesAsDisplay()
        {
            var categories = _context.Categories.AsNoTracking()
               .Select(c => new DisplayCategoryVM
               {
                   Id = c.Id,
                   Name = c.Name,
                   Description = c.Description,
                   BookCount = c.Books.Count
               });
            return categories;
        }    
    }
}
