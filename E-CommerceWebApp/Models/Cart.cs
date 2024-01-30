namespace E_CommerceWebApp.Models
{
    public class Cart
    {
        public int Id { get; set; }
        // user
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        // date
        public DateTime TimeCreated { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
