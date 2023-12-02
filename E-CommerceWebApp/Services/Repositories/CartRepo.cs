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
        // add exception here
        public void CreateNewCart()
        {
            var existingCartItem = GetCart(1);
            if (existingCartItem != null) return;

            _dbContext.Carts.Add(new Cart { timeCreated = DateTime.Now });
            _dbContext.SaveChanges();
        }
        public Cart GetCart(int cartID)
        {
            return _dbContext.Carts.FirstOrDefault(c => c.Id == cartID);
        }
        public IEnumerable<CartItem> GetAllCartItems()
        {
           return _dbContext.CartItems.ToList();
        }
        public void AddOrUpdateCartItem(Product product)
        {
            // Check if the entity already exists in the database
            var existingCartItem = _dbContext.Set<CartItem>().FirstOrDefault(i => i.ProductID == product.ProductID);

            if (existingCartItem == null)
            {
                // The entity does not exist, so add it to the database
                _dbContext.Set<CartItem>().Add(new CartItem 
                {
                    ProductID=product.ProductID,
                    Amount=1,
                    SinglePrice=product.Price,
                    CartID=1,
                });
            }
            else
            {
                // The entity already exists, perform difference logic here
                existingCartItem.Amount = +1;
            }

            // Save changes to the database
            _dbContext.SaveChanges();
        }

        public void RemoveCartItem(int itemID)
        {
            throw new NotImplementedException();
        }

        public void UpdateCartItem(int itemID)
        {
            throw new NotImplementedException();
        }

        
    }
}
