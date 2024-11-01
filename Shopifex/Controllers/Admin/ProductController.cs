using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopifex.Constants;
using Shopifex.Models;
using Shopifex.Services;

namespace Shopifex.Controllers.Admin
{
    [Authorize(Roles = Roles.Admin)]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]/{id?}")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly ShopifexContext _context;

        public ProductController(ShopifexContext context, ProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        public IActionResult Index(int? categoryId)
        {
            var products = _context.Products
                                   .Include(p => p.Category)
                                   .AsQueryable();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name", categoryId);

            return View(products.ToList());
        }

        public IActionResult Create()
        {
            ViewData["Categories"] = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            ViewData["Categories"] = _context.Categories.ToList();
            if (ModelState.IsValid)
            {
                var existingProduct = _context.Products.FirstOrDefault(p => p.Name == product.Name);
                if (existingProduct != null)
                {
                    ModelState.AddModelError("Name", "Produkt o tej nazwie już istnieje.");
                    return View(product);
                }

                _context.Products.Add(product);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public IActionResult Edit(int id)
        {
            ViewData["Categories"] = _context.Categories.ToList();
            var product = _productService.GetProductById(id);
            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            ViewData["Categories"] = _context.Categories.ToList();
            if (ModelState.IsValid)
            {
                var existingProduct = _context.Products.FirstOrDefault(p => p.Name == product.Name && p.Id != product.Id);
                if (existingProduct != null)
                {
                    ModelState.AddModelError("Name", "Produkt o tej nazwie już istnieje.");
                    return View(product);
                }

                _context.Products.Update(product);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _productService.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
