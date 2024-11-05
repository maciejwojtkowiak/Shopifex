using System.ComponentModel.DataAnnotations;

namespace Shopifex.Models
{
    public class Category
    {
        public int Id { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
        [Display(Name = "Nazwa kategorii")]
        [Required(ErrorMessage = "Nazwa kategorii jest wymagana")]
        [MaxLength(100, ErrorMessage = "Nazwa kategorii nie może być dłuższa niż 100 znaków")]

        public string Name { get; set; } = null!;
    }
}
