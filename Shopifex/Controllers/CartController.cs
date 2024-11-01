using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shopifex.Models;
using Shopifex.Services;

namespace Shopifex.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly ProductService _productService;
        private readonly ShopifexContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(CartService cartService, ProductService productService, ShopifexContext context, UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _productService = productService;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var cart = _cartService.GetCart();
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            var product = _productService.GetProductById(productId);
            _context.Products.Attach(product);
            _cartService.AddToCart(product, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var product = _productService.GetProductById(productId);
            _cartService.RemoveFromCart(product);
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult SavedCarts()
        {
            var carts = _cartService.GetUserSavedCarts();
            var savedCarts = carts.Where(c => c.IsSavedByUser);
            return View("SavedCarts", savedCarts);
        }


        [HttpPost]
        [Authorize]
        public IActionResult SaveCart()
        {
            var cart = _cartService.GetCart();
            cart.IsSavedByUser = true;
            _cartService.AddToDatabase(cart);
            return RedirectToAction("SavedCarts");
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var cart = _cartService.GetSavedCartById(id);
            if (cart == null)
            {
                return NotFound();
            }
            if (cart.UserId != _userManager.GetUserId(User))
            {
                return Forbid();
            }
            return View(cart);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteSavedCart(int id)
        {
            var cart = _cartService.GetSavedCartById(id);
            if (cart == null)
            {
                return NotFound();
            }

            _cartService.DeleteSavedCart(id);
            return RedirectToAction("SavedCarts");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult RestoreSavedCart(int id)
        {
            _cartService.ChangeCart(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, string action)
        {
            var cart = _cartService.GetCart();
            var cartItem = cart.Items.FirstOrDefault(item => item.Product.Id == productId);

            if (cartItem != null)
            {
                if (action == "increase")
                {
                    cartItem.Quantity++;
                }
                else if (action == "decrease" && cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                }
                else if (action == "decrease" && cartItem.Quantity == 1)
                {
                    cart.Items.Remove(cartItem);
                }
            }
            _cartService.SaveCartToSession(cart);
            return RedirectToAction("Index");
        }
    }
}