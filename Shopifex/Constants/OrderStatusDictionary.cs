namespace Shopifex.Constants
{
    public class OrderStatusDictionary
    {
        public static string GetOrderStatus(OrderStatusEnum orderStatus)
        {
            switch (orderStatus)
            {
                case OrderStatusEnum.Completed:
                    return "Zakończone";
                default:
                    return "W trakcie";
            }
        }
    }
}
