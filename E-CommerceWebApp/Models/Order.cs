namespace E_CommerceWebApp.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime TimeCreated { get; set; } = DateTime.Now;
        public OrderStatus OrderStatus { get; set; }
        public DateTime TimeConfirmed { get; set; }
        // user
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        //OrderItems
        public IEnumerable<OrderItem> OrderItems { get; set; }
    }

    public enum OrderStatus
    {
        Waiting,
        Confirming,
        Shipping,
        Delayed,
        Cancelled,
        Delivered
    }
}
