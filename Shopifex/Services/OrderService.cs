using Microsoft.EntityFrameworkCore;
using Shopifex.Models;

namespace Shopifex.Services
{
    public class OrderService
    {
        private readonly ShopifexContext _context;

        public OrderService(ShopifexContext context)
        {
            _context = context;
        }

        public void AddOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public IEnumerable<Order> GetUserOrders(string userId)
        {
            return _context.Orders.Include(o => o.Cart).ThenInclude(i => i.Items).ThenInclude(p => p.Product).Where(o => o.UserId == userId).ToList();
        }

        public int GetTotalOrders()
        {
            return _context.Orders.Count();
        }

        public int GetGuestOrdersCount()
        {
            return _context.Orders
                           .Where(o => o.UserId == null)
                           .Count();
        }

        public int GetUserOrdersCount()
        {
            return _context.Orders
                           .Where(o => o.UserId != null)
                           .Count();
        }

        public List<TopProduct> GetTopProducts(int count)
        {
            return _context.CartItems
                .Include(ci => ci.Product)
                .GroupBy(ci => ci.ProductId)
                .Select(group => new TopProduct
                {
                    ProductName = group.First().Product.Name,
                    QuantitySold = group.Sum(ci => ci.Quantity)
                })
                .OrderByDescending(ps => ps.QuantitySold)
                .Take(count)
                .ToList();
        }
    }
}
