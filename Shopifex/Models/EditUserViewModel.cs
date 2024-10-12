using System.ComponentModel.DataAnnotations;

namespace Shopifex.Models
{
    public class EditUserViewModel
    {
        [Required(ErrorMessage = "Adres e-mail jest wymagany.")]
        [EmailAddress(ErrorMessage = "Podaj poprawny adres e-mail.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Rola jest wymagana.")]
        public string Role { get; set; }
    }
}
