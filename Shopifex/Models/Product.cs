using System.ComponentModel.DataAnnotations;

namespace Shopifex.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa produktu jest wymagana.")]
        [StringLength(100, ErrorMessage = "Nazwa produktu nie może być dłuższa niż 100 znaków.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Opis produktu jest wymagany.")]
        [StringLength(500, ErrorMessage = "Opis produktu nie może być dłuższy niż 500 znaków.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Cena produktu jest wymagana.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cena musi być większa od zera.")]
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; } = null!;
    }
}
