using Microsoft.EntityFrameworkCore;
using Shopifex.Models;

namespace Shopifex.Services
{
    public class ProductService
    {
        private readonly ShopifexContext _context;

        public ProductService(ShopifexContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAllProducts() => _context.Products.Include(p => p.Category).ToList();

        public Product GetProductById(int id) => _context.Products.Find(id);

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            var product = GetProductById(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}
