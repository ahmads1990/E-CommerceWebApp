namespace E_CommerceWebApp.Services.Repositories.Interfaces
{
    public interface ICartRepo
    {
        // Cart
        public void CreateNewCart();
        public Cart GetCart(int cartID);
        // CartItems
        public IEnumerable<CartItem> GetAllCartItems();
        public void AddOrUpdateCartItem(Product product);
        public void UpdateCartItem(int itemID);
        public void RemoveCartItem(int itemID);
    }
}
