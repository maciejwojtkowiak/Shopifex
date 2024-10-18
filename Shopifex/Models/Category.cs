namespace Shopifex.Models
{
    public class Category
    {
        public int Id { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public string Name { get; set; } = null!;
    }
}
