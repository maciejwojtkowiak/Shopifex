using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shopifex.Models;
using Shopifex.Services;

namespace Shopifex.Controllers
{
    namespace Shopifex.Controllers
    {
        public class ProductController : Controller
        {
            private readonly ProductService _productService;
            private readonly ShopifexContext _context;

            public ProductController(ProductService productService, ShopifexContext context)
            {
                _productService = productService;
                _context = context;
            }

            public IActionResult Index(int? categoryId)
            {
                var products = _productService.GetAllProducts();

                if (categoryId.HasValue)
                {
                    products = products.Where(p => p.CategoryId == categoryId.Value).ToList();
                }

                ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name", categoryId);

                return View(products);
            }
        }
    }
}