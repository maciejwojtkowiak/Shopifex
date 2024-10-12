using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            var products = _productService.GetAllProducts();
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
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
            var product = _productService.GetProductById(id);
            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
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
