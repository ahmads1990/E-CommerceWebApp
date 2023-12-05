using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        public Cart GetCompleteUserCart(string userID)
        {
            // checking if the user already have a cart
            var existingCart = _dbContext.Carts
                                .Include(c => c.CartItems)
                                .ThenInclude(ci => ci.Product)
                                .ThenInclude(p=>p.ProductImage)
                                .FirstOrDefault(c => c.UserId == userID);
            return existingCart;
        }
        public IEnumerable<CartItem> GetAllCartItems()
        {
            return _dbContext.CartItems
                 .Include(p => p.Product)
                 .Include(im => im.Product.ProductImage)
                 .ToList();
        }
        public CartItem GetCartItemByID(int itemID)
        {
            return _dbContext.CartItems.FirstOrDefault(i => i.ID == itemID);
        }
        public void AddOrUpdateCartItem(int productID)
        {
            // Check if the entity already exists in the database
            var existingCartItem = _dbContext.Set<CartItem>().FirstOrDefault(i => i.ProductID == productID);

            if (existingCartItem == null)
            {
                // The entity does not exist, so add it to the database
                _dbContext.Set<CartItem>().Add(new CartItem
                {
                    ProductID = productID,
                    Amount = 1,
                    SinglePrice = 0,
                    CartID = 1,
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
        public void UpdateCartItemAmount(int itemID, int amount)
        {
            var existingCartItem = GetCartItemByID(itemID);
            if (existingCartItem != null && amount >= 1)
            {
                existingCartItem.Amount = amount;
                _dbContext.CartItems.Update(existingCartItem);
                _dbContext.SaveChanges();
            }
        }
        public void RemoveCartItem(int itemID)
        {
            // Check if the entity already exists in the database
            var existingCartItem = GetCartItemByID(itemID);
            if (existingCartItem != null)
            {
                _dbContext.CartItems.Remove(existingCartItem);
                _dbContext.SaveChanges();
            }
        }
    }
}