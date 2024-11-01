using System.ComponentModel.DataAnnotations;

namespace Shopifex.Models
{
    public class Category
    {
        public int Id { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
        [Display(Name = "Nazwa kategorii")]
        public string Name { get; set; } = null!;
    }
}
