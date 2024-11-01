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
        private readonly OrderService _orderService;

        public OrderController(ShopifexContext context, ProductService productService, OrderService orderService)
        {
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

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Products"] = _productService.GetAllProducts();
            return View(new Order { Cart = new Cart() });
        }

        [HttpPost]
        public IActionResult Create(Order order, int[] selectedProductIds, int[] quantities)
        {
            if (ModelState.IsValid)
            {
                order.Cart = new Cart();

                for (int i = 0; i < selectedProductIds.Length; i++)
                {
                    var productId = selectedProductIds[i];
                    var quantity = quantities[i];

                    order.Cart.Items.Add(new CartItem
                    {
                        ProductId = productId,
                        Quantity = quantity
                    });
                }

                _orderService.AddOrder(order);


                return RedirectToAction(nameof(Index));
            }

            ViewData["Products"] = _productService.GetAllProducts();
            return View(order);
        }
    }
}