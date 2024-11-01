using System.ComponentModel.DataAnnotations;

namespace Shopifex.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa produktu jest wymagana.")]
        [StringLength(100, ErrorMessage = "Nazwa produktu nie może być dłuższa niż 100 znaków.")]
        [Display(Name = "Nazwa produktu")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Opis produktu jest wymagany.")]
        [StringLength(500, ErrorMessage = "Opis produktu nie może być dłuższa niż 500 znaków.")]
        [Display(Name = "Opis produktu")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Cena produktu jest wymagana.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cena musi być większa od zera.")]
        [Display(Name = "Cena produktu")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Zdjęcie produktu jest wymagane")]
        [Display(Name = "Zdjęcie produktu")]
        public string ImageData { get; set; } = null!;

        [Required(ErrorMessage = "Kategoria jest wymagana")]
        [Display(Name = "Kategoria")]
        public int? CategoryId { get; set; }

        public Category? Category { get; set; } = null!;
    }
}
