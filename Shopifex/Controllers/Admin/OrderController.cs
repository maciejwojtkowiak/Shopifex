using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopifex.Constants;
using Shopifex.Models;
using Shopifex.Services;

namespace Shopifex.Controllers.Admin
{
    [Authorize(Roles = Roles.Admin)]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]/{id?}")]
    public class OrderController : Controller
    {
        private readonly ShopifexContext _context;
        private readonly ProductService _productService;
        private readonly CartService _cartService;
        private readonly OrderService _orderService;

        public OrderController(ShopifexContext context, CartService cartService, ProductService productService, OrderService orderService)
        {
            _cartService = cartService;
            _context = context;
            _productService = productService;
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            var orders = _context.Orders.ToList();
            return View(orders);
        }

        public IActionResult Details(int id)
        {
            var order = _context.Orders.Include(o => o.Cart).ThenInclude(c => c.Items).ThenInclude(i => i.Product)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public IActionResult Edit(int id)
        {
            var order = _context.Orders.Find(id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        public IActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Orders.Update(order);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(order);
        }

        public IActionResult Delete(int id)
        {
            var order = _context.Orders.Find(id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var order = _context.Orders.Find(id);

            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            var viewModel = new OrderCreateViewModel
            {
                Products = _productService.GetAllProducts().Select(p => new ProductSelectionViewModel
                {
                    ProductId = p.Id,
                    ProductName = p.Name
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(OrderCreateViewModel model)
        {

            var selectedProducts = model.Products
                .Where(p => p.IsSelected)
                .ToList();

            if (!selectedProducts.Any())
            {
                ViewData["noSelectedProducts"] = "Musisz wybrać przynajmniej jeden produkt z ilością większą niż 0.";
                return View(model);
            }
            ViewData["noSelectedProducts"] = null;
            foreach (var product in model.Products)
            {
                if (product.IsSelected && product.Quantity <= 0)
                {
                    ModelState.AddModelError($"Products[{model.Products.IndexOf(product)}].Quantity", "Ilość musi być większa od 0.");
                }
            }
            if (ModelState.IsValid)
            {
                var cart = new Cart
                {
                    Items = selectedProducts.Select(p => new CartItem
                    {
                        Product = _productService.GetProductById(p.ProductId),
                        Quantity = p.Quantity,
                    }).ToList()
                };
                _cartService.AddToDatabase(cart);
                var order = new Order
                {
                    Address = model.Address,
                    Cart = cart,
                    Email = model.Email,
                    FullName = model.FullName,
                    Phone = model.Phone,
                    Status = model.Status,
                    UserId = null,
                };

                _orderService.AddOrder(order);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}