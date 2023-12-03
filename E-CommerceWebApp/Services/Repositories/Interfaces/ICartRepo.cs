namespace E_CommerceWebApp.Services.Repositories.Interfaces
{
    public interface ICartRepo
    {
        // Cart
        public void CreateNewCart();
        public Cart GetCart(int cartID);
        // CartItems
        public IEnumerable<CartItem> GetAllCartItems();
        public CartItem GetCartItemByID(int itemID);
        public void AddOrUpdateCartItem(int productID);
        public void UpdateCartItemAmount(int itemID, int amount);
        public void RemoveCartItem(int itemID);
    }
}
