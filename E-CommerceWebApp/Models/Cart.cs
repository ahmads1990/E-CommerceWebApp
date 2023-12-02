namespace E_CommerceWebApp.Models
{
    public class Cart
    {
        public int Id { get; set; }
        //user to do
        // date
        public DateTime timeCreated { get; set; }
        public IEnumerable<CartItem> cartItems { get; set; }
    }
}
