namespace E_CommerceWebApp.Models
{
    public class Cart
    {
        public int Id { get; set; }
        //user
        public List<CartItem> cartItems { get; set; }
    }
}
