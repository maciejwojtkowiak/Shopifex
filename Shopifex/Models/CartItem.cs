using System.ComponentModel.DataAnnotations;

namespace Shopifex.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Identyfikator produktu jest wymagany")]
        public int ProductId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Ilość musi wynosić co najmniej 1.")]
        public int Quantity { get; set; }
        public Product Product { get; set; }
        public int CartId { get; set; }
    }
}