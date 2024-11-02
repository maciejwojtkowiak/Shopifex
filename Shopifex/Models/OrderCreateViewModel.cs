﻿using Shopifex.Constants;
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

        public OrderStatusEnum Status { get; set; }

        [Required(ErrorMessage = "Musisz wybrać przynajmniej jeden produkt.")]
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
    }
}