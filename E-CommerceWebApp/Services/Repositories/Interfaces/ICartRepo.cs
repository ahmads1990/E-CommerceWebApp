namespace E_CommerceWebApp.Services.Repositories.Interfaces
{
    public interface ICartRepo
    {
        // Cart
        public Cart GetUserCart(string userID);
        public Cart CreateUserCart(string userID);
        public Cart GetCompleteUserCart(int cartId);
        public bool ClearCartItems(int cartId);
        // CartItems
        public IEnumerable<CartItem> GetAllCartItems(int cartId);
        public CartItem GetCartItemByID(int itemId);
        public void AddOrUpdateCartItem(int cartId, int productID);
        // extra function used
        public void UpdateCartItemAmount(int cartItemID, int amount);
        public void RemoveCartItem(int cartItemID);
    }
}
