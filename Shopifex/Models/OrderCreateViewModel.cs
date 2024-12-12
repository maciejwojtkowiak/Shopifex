using Shopifex.Constants;
using System.ComponentModel.DataAnnotations;

namespace Shopifex.Models
{
    public class OrderCreateViewModel
    {
        [Required(ErrorMessage = "Imię i nazwisko są wymagane.")]
        [Display(Name = "Imię i nazwisko")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Adres jest wymagany.")]
        [Display(Name = "Adres")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Numer telefonu jest wymagany.")]
        [Phone(ErrorMessage = "Podaj poprawny numer telefonu.")]
        [Display(Name = "Numer telefonu")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Adres e-mail jest wymagany.")]
        [EmailAddress(ErrorMessage = "Podaj poprawny adres e-mail.")]
        [Display(Name = "Adres e-mail")]
        public string Email { get; set; }

        [EnumValidation(typeof(OrderStatusEnum), ErrorMessage = "Nieprawidłowy status zamówienia.")]
        public OrderStatusEnum? Status { get; set; }

        [Required(ErrorMessage = "Wybór użytkownika jest wymagany.")]
        [Display(Name = "Użytkownik")]
        public string UserId { get; set; }

        public List<ProductSelectionViewModel> Products { get; set; } = new List<ProductSelectionViewModel>();
    }

    public class ProductSelectionViewModel
    {
        public int ProductId { get; set; }
        [Display(Name = "Produkt")]
        public string ProductName { get; set; }
        public bool IsSelected { get; set; }
        [Display(Name = "Ilość")]
        public int Quantity { get; set; }
        [Display(Name = "Cena")]
        public decimal Price { get; set; }
    }

    public class EnumValidationAttribute : ValidationAttribute
    {
        private readonly Type _enumType;

        public EnumValidationAttribute(Type enumType)
        {
            _enumType = enumType;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || value?.ToString()?.Length == 0)
            {
                return new ValidationResult("Pole jest wymagane.");
            }

            if (!Enum.IsDefined(_enumType, value))
            {
                return new ValidationResult($"Nieprawidłowa wartość dla {_enumType.Name}.");
            }

            return ValidationResult.Success;
        }
    }
}
