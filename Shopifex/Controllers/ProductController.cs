using Microsoft.AspNetCore.Mvc;
using Shopifex.Services;

namespace Shopifex.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View(_productService.GetAllProducts());
        }
    }
}
