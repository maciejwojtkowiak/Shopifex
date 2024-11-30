using System.ComponentModel.DataAnnotations;

namespace Shopifex.Models
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Adres e-mail jest wymagany.")]
        [EmailAddress(ErrorMessage = "Podaj poprawny adres e-mail.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        [StringLength(100, ErrorMessage = "Hasło musi mieć co najmniej 6 i maksymalnie 100 znaków.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Rola jest wymagana.")]
        [Display(Name = "Rola")]
        public string Role { get; set; }
    }
}
