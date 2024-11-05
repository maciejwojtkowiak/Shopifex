using Shopifex.Constants;
using System.ComponentModel.DataAnnotations;

namespace Shopifex.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Imię i nazwisko są wymagane.")]
        [StringLength(200, ErrorMessage = "Imię i nazwisko mogą mieć maksymalnie 100 znaków.")]
        [Display(Name = "Imię i nazwisko")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Adres jest wymagany.")]
        [StringLength(300, ErrorMessage = "Adres może mieć maksymalnie 200 znaków.")]
        [Display(Name = "Adres")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Numer telefonu jest wymagany.")]
        [Phone(ErrorMessage = "Podaj poprawny numer telefonu.")]
        [Display(Name = "Numer telefonu")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Adres e-mail jest wymagany.")]
        [EmailAddress(ErrorMessage = "Podaj poprawny adres e-mail.")]
        [MaxLength(256, ErrorMessage = "Adres e-mail nie może być dłuższy niż 256 znaków.")]
        public string Email { get; set; }

        public OrderStatusEnum Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Cart? Cart { get; set; } = null!;

        public string? UserId { get; set; } = null!;

        public ApplicationUser? User { get; set; } = null!;
    }
}
