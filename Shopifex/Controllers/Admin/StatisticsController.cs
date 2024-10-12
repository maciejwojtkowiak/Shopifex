using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopifex.Constants;
using Shopifex.Models;
using Shopifex.Services;

namespace Shopifex.Controllers.Admin
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
    [Route("Admin/[controller]/[action]/{id?}")]
    public class StatisticsController : Controller
    {
        private readonly ShopifexContext _context;
        private readonly OrderService _orderService;

        public StatisticsController(ShopifexContext context, OrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            var totalOrders = _context.Orders.Count();
            var ordersByGuests = _context.Orders.Count(o => o.UserId == null);
            var ordersByUsers = totalOrders - ordersByGuests;
            var topProducts = _orderService.GetTopProducts(5);
            var guestOrders = _context.Orders
           .Where(o => o.UserId == null)
           .Include(o => o.Cart)
           .ThenInclude(c => c.Items)
           .ThenInclude(i => i.Product)
           .ToList();

            var userOrders = _context.Orders
                .Where(o => o.UserId != null)
                .Include(o => o.Cart)
                .ThenInclude(c => c.Items)
                .ThenInclude(i => i.Product)
                .ToList();

            var totalOrderPriceByGuests = guestOrders.Sum(o => o.Cart?.CalculateTotal() ?? 0);
            var averageOrderPriceByGuests = ordersByGuests > 0 ? totalOrderPriceByGuests / ordersByGuests : 0;

            var totalOrderPriceByUsers = userOrders.Sum(o => o.Cart?.CalculateTotal() ?? 0);
            var averageOrderPriceByUsers = ordersByUsers > 0 ? totalOrderPriceByUsers / ordersByUsers : 0;

            var totalOrderPrice = totalOrderPriceByGuests + totalOrderPriceByUsers;
            var averageOrderPrice = totalOrders > 0 ? totalOrderPrice / totalOrders : 0;

            return View(new Statistics
            {
                TotalOrders = totalOrders,
                OrdersByGuests = ordersByGuests,
                OrdersByUsers = ordersByUsers,
                TotalOrderPriceByGuests = totalOrderPriceByGuests,
                AverageOrderPriceByGuests = averageOrderPriceByGuests,
                TotalOrderPriceByUsers = totalOrderPriceByUsers,
                AverageOrderPriceByUsers = averageOrderPriceByUsers,
                TotalOrderPrice = totalOrderPrice,
                AverageOrderPrice = averageOrderPrice,
                TopProducts = topProducts
            });
        }
    }
}
