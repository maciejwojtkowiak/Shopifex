using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shopifex.Models;
using Shopifex.Services;

namespace Shopifex.Controllers
{
    public class OrderController : Controller
    {
        private readonly CartService _cartService;
        private readonly OrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(CartService cartService, OrderService orderService, UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _orderService = orderService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var cart = _cartService.GetCart();

            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new Order
            {
                Cart = cart
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Order model)
        {
            var cart = _cartService.GetCart();
            _cartService.AddToDatabase(cart);
            model.Cart = cart;
            model.Status = Constants.OrderStatusEnum.InProgress;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData["OrderSubmitted"] = true;
            model.UserId = _userManager.GetUserId(User);
            _orderService.AddOrder(model);

            return View("OrderConfirmation", model);
        }

        [Authorize]
        public IActionResult OrderHistory()
        {
            var orders = _orderService.GetUserOrders(_userManager.GetUserId(User));
            return View(orders);
        }

        [Authorize]
        [HttpGet("/Order/Details/{id}")]
        public IActionResult Details(int id)
        {
            var order = _orderService.GetUserOrders(_userManager.GetUserId(User)).ToList().Find(o => o.Id == id);
            if (order == null)
            {
                return Forbid();
            }
            return View(order);
        }

        public IActionResult OrderConfirmation(Order model)
        {
            if (TempData["OrderSubmitted"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            TempData["OrderSubmitted"] = null;
            _cartService.ClearCart();
            return View(model);
        }
    }
}