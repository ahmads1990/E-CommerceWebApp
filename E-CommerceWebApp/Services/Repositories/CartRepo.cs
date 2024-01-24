using Microsoft.EntityFrameworkCore;

namespace E_CommerceWebApp.Services.Repositories
{
    // This class is responsible for creating a cart and managing cartItems inside the cart
    public class CartRepo : ICartRepo
    {
        ApplicationDBContext _dbContext;
        public CartRepo(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Cart GetUserCart(string userID)
        {
            return _dbContext.Carts.FirstOrDefault(c => c.UserId == userID);
        }

        public Cart CreateUserCart(string userID)
        {
            // checks if the user already has a cart if so return 
            if (GetUserCart(userID) != null)
                return null;

            var newCart = new Cart
            {
                UserId = userID,
                TimeCreated = DateTime.Now,
                CartItems = new List<CartItem>()
            };

            _dbContext.Carts.Add(newCart);
            _dbContext.SaveChanges();
            return newCart;
        }
        public Cart GetCompleteUserCart(int cartId)
        {
            //  getting user cart and including all product data
            var completeUserCart = _dbContext.Carts
                                .Where(c => c.Id == cartId)
                                .Include(c => c.CartItems)
                                    .ThenInclude(ci => ci.Product)
                                        .ThenInclude(p => p.ProductImage)
                                .Single();

            return completeUserCart;
        }
        public IEnumerable<CartItem> GetAllCartItems(int cartId)
        {
            return _dbContext.CartItems
                .Where(ci => ci.CartID == cartId)
                    .Include(p => p.Product)
                        .ThenInclude(im => im.ProductImage)
                 .ToList();
        }
        public CartItem GetCartItemByID(int itemId)
        {
            return _dbContext.CartItems.FirstOrDefault(i => i.ID == itemId);
        }
        public void AddOrUpdateCartItem(int cartId, int productID)
        {
            // Check if the entity already exists in the database
            var existingCartItem = _dbContext.CartItems
                                    .FirstOrDefault(i => i.ProductID == productID && i.CartID == cartId);

            if (existingCartItem == null)
            {
                // The entity does not exist, so add it to the database
                _dbContext.Set<CartItem>().Add(new CartItem
                {
                    ProductID = productID,
                    Amount = 1,
                    SinglePrice = 0, // todo delete this prop
                    CartID = cartId,
                });
            }
            else
            {
                // The entity already exists, perform difference logic here
                existingCartItem.Amount += 1;
            }
            // Save changes to the database
            _dbContext.SaveChanges();
        }
        // extra method for sending amount of products instead of increment by one
        public void UpdateCartItemAmount(int cartItemID, int amount)
        {
            var existingCartItem = GetCartItemByID(cartItemID);
            if (existingCartItem != null && amount >= 1)
            {
                existingCartItem.Amount = amount;
                _dbContext.CartItems.Update(existingCartItem);
                _dbContext.SaveChanges();
            }
        }
        public void RemoveCartItem(int cartItemID)
        {
            // Check if the entity already exists in the database
            var existingCartItem = GetCartItemByID(cartItemID);
            if (existingCartItem != null)
            {
                _dbContext.CartItems.Remove(existingCartItem);
                _dbContext.SaveChanges();
            }
        }
    }
}