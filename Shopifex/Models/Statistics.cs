namespace Shopifex.Models
{
    public class Statistics
    {
        public int TotalOrders { get; set; }
        public int OrdersByGuests { get; set; }
        public int OrdersByUsers { get; set; }
        public decimal TotalOrderPriceByGuests { get; set; }
        public decimal AverageOrderPriceByGuests { get; set; }
        public decimal TotalOrderPriceByUsers { get; set; }
        public decimal AverageOrderPriceByUsers { get; set; }
        public decimal TotalOrderPrice { get; set; }
        public decimal AverageOrderPrice { get; set; }
        public List<TopProduct> TopProducts { get; set; }
    }

    public class TopProduct
    {
        public string ProductName { get; set; }
        public int QuantitySold { get; set; }
    }
}
