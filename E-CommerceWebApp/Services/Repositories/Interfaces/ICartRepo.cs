namespace E_CommerceWebApp.Services.Repositories.Interfaces
{
    public interface ICartRepo
    {
        // Cart
        public Cart GetUserCart(string userID);
        public Cart CreateUserCart(string userID);
        public Cart GetCompleteUserCart(string userID);
        // CartItems
        public IEnumerable<CartItem> GetAllCartItems();
        public CartItem GetCartItemByID(int itemID);
        public void AddOrUpdateCartItem(int productID);
        public void UpdateCartItemAmount(int itemID, int amount);
        public void RemoveCartItem(int itemID);
    }
}
