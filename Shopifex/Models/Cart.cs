

namespace Shopifex.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        public bool IsSavedByUser { get; set; }

        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();

        public void AddItem(Product product, int quantity)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == product.Id);
            if (item == null)
            {
                Items.Add(new CartItem
                {
                    ProductId = product.Id,
                    Quantity = quantity,
                    CartId = Id,
                    Product = product
                });
            }
            else
            {
                item.Quantity += quantity;
            }
        }

        public void RemoveItem(Product product)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == product.Id);
            if (item != null)
            {
                Items.Remove(item);
            }
        }

        public decimal CalculateTotal()
        {
            return Items.Sum(i => i.Product.Price * i.Quantity);
        }
    }
}
