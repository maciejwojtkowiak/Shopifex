namespace Shopifex.Controllers.Admin
{
    using global::Shopifex.Constants;
    using global::Shopifex.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    namespace Shopifex.Controllers
    {
        [Authorize(Roles = Roles.Admin)]
        [Area("Admin")]
        [Route("Admin/[controller]/[action]/{id?}")]
        public class CategoryController : Controller
        {
            private readonly ShopifexContext _context;

            public CategoryController(ShopifexContext context)
            {
                _context = context;
            }

            public IActionResult Index()
            {
                var categories = _context.Categories.ToList();
                return View(categories);
            }

            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Create(Category category)
            {
                if (ModelState.IsValid)
                {
                    _context.Categories.Add(category);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return View(category);
            }

            public IActionResult Edit(int id)
            {
                var category = _context.Categories.Find(id);
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Edit(Category category)
            {
                if (ModelState.IsValid)
                {
                    _context.Categories.Update(category);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return View(category);
            }

            public IActionResult Delete(int id)
            {
                var category = _context.Categories.Find(id);
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }

            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public IActionResult DeleteConfirmed(int id)
            {
                var category = _context.Categories.Find(id);
                if (category != null)
                {
                    _context.Categories.Remove(category);
                    _context.SaveChanges();
                }
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
