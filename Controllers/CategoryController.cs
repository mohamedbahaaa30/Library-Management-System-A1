using Library_Management_System.Models.DTO.Category;
using Library_Management_System.Models.VM.Category;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index()
        {
            var results = _context.Categories.ToList();
            return results.Count() > 0 ? View(results) : View("Empty");
        }
        [HttpGet]
        public IActionResult Create()
        {
            // Populate the dropdown lists for authors and categories
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreateCategoryDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var Exist = _context.Categories.FirstOrDefault(c => c.Name == model.Name);
            if (Exist != null)
            {
                ModelState.AddModelError("Name", "A category with this name already exists.");
                return View(model);
            }
            Category category = new Category
            {
                Name = model.Name,
                Description = model.Description
            };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            EditCategoryVM editCategoryVM = new EditCategoryVM
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
            return View(editCategoryVM);
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateCategoryDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var Exist = await _context.Categories.FindAsync(model.Id);
            if (Exist == null)
                return NotFound();
            var takenName = await _context.Categories.AnyAsync(c => c.Name == model.Name && c.Id != model.Id );
            if (takenName)
            {
                ModelState.AddModelError("Name", "A category with this name already exists.");
                return View(model);
            }
            Exist.Name = model.Name;
            Exist.Description = model.Description;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
