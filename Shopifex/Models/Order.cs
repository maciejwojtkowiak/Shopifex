using System.ComponentModel.DataAnnotations;

namespace Shopifex.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Imię i nazwisko są wymagane.")]
        [StringLength(100, ErrorMessage = "Imię i nazwisko mogą mieć maksymalnie 100 znaków.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Adres jest wymagany.")]
        [StringLength(200, ErrorMessage = "Adres może mieć maksymalnie 200 znaków.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Numer telefonu jest wymagany.")]
        [Phone(ErrorMessage = "Podaj poprawny numer telefonu.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Adres e-mail jest wymagany.")]
        [EmailAddress(ErrorMessage = "Podaj poprawny adres e-mail.")]
        public string Email { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Cart? Cart { get; set; } = null!;

        public string? UserId { get; set; } = null!;

        public ApplicationUser? User { get; set; } = null!;
    }
}
