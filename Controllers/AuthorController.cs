
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace Library_Management_System.Controllers
{
    public class AuthorController : Controller
    {
        public Context _context;
        public AuthorController(Context context)
        {
            this._context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var results = _context.Authors.ToList();

            return results.Count() > 0 ? View(results) : View("Empty");
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
        public async Task<IActionResult> Add(CreateAuthorDto model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var exist = await _context.Authors.AnyAsync(A => A.Name == model.Name);
            if (exist)
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
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var author = _context.Authors.FirstOrDefault(a => a.Id == id);
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
            return View(editAuthorVM);
        }
        [HttpPut]
        public async Task<IActionResult> Update(EditAuthorDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var exist = await _context.Authors.FindAsync(model.Id);
            if(exist == null)
            {
                return NotFound();
            }
            var takenName = await _context.Authors.AnyAsync(A => A.Name == model.Name && A.Id != model.Id);
            if (takenName)
            {
                ModelState.AddModelError("Name", "Author with this name alread exist");
                return View(model);
            }
            exist.Name = model.Name;
            exist.Bio = model.Bio;

            await _context.SaveChangesAsync();
            return RedirectToAction("GetById" , new { id = exist.Id });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            var item = await _context.Authors.FindAsync(Id);
            if (item == null)
                return NotFound();
             _context.Authors.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }



    }
}
