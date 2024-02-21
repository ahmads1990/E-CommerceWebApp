namespace E_CommerceWebApp.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int Amount { get; set; }
        public float SinglePrice { get; set; }
        //Order
        public int OrderId { get; set; }
        public Order Order { get; set; }
        //Product
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
