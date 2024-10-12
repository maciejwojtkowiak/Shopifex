using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shopifex.Models;
using System.Text.Json;

namespace Shopifex.Services
{
    public class CartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ShopifexContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartService(IHttpContextAccessor httpContextAccessor, ShopifexContext context, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _userManager = userManager;
        }

        private Cart GetCartFromSession()
        {
            var context = _httpContextAccessor.HttpContext;
            var sessionCart = context.Session.GetString("Cart");
            if (string.IsNullOrEmpty(sessionCart))
            {
                var cart = new Cart();
                cart.UserId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
                return cart;
            }

            return JsonSerializer.Deserialize<Cart>(sessionCart);
        }

        public void ChangeCart(int cartId)
        {
            var savedCart = GetSavedCartById(cartId);
            var newCart = new Cart
            {
                UserId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User),
                Items = savedCart.Items.Select(item => new CartItem
                {
                    ProductId = item.ProductId,
                    Product = item.Product,
                    Quantity = item.Quantity
                }).ToList()
            };
            SaveCartToSession(newCart);
        }

        public void SaveCartToSession(Cart cart)
        {
            var context = _httpContextAccessor.HttpContext;
            context.Session.SetString("Cart", JsonSerializer.Serialize(cart));
        }

        public void AddToCart(Product product, int quantity)
        {
            var cart = GetCartFromSession();
            cart.AddItem(product, quantity);
            SaveCartToSession(cart);
        }

        public void AddToDatabase(Cart cart)
        {
            _context.Carts.Add(cart);
            cart.Items.ToList().ForEach(i =>
            {
                _context.Products.Attach(i.Product);
            });
            _context.SaveChanges();
        }

        public Cart GetSavedCartById(int id)
        {
            return _context.Carts.Include(c => c.Items).ThenInclude(i => i.Product).FirstOrDefault(c => c.Id == id);
        }

        public void DeleteSavedCart(int id)
        {
            var cart = _context.Carts.Include(c => c.Items).FirstOrDefault(c => c.Id == id);
            _context.Carts.Remove(cart);
            _context.SaveChanges();
        }

        public void RemoveFromCart(Product product)
        {
            var cart = GetCartFromSession();
            cart.RemoveItem(product);
            SaveCartToSession(cart);
        }

        public Cart GetCart()
        {
            return GetCartFromSession();
        }

        public void ClearCart()
        {
            var cart = GetCartFromSession();
            cart.Items.Clear();
            SaveCartToSession(cart);
        }

        public IEnumerable<Cart> GetUserSavedCarts()
        {
            var carts = _context.Carts.Include(c => c.Items).ThenInclude(i => i.Product).Where(c => c.UserId == _userManager.GetUserId(_httpContextAccessor.HttpContext.User));
            return carts;
        }
    }
}
