
using E_CommerceWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceWebApp.Services.Repositories
{
    public class OrderRepo : IOrderRepo
    {
        ApplicationDBContext _dbContext;
        public OrderRepo(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Order> ConfirmCartToOrderAsync(Cart cart)
        {
            var newOrder = new Order
            {
                OrderStatus = OrderStatus.Waiting,
                TimeCreated = DateTime.Now,
                UserId = cart.UserId,
                OrderItems = cart.CartItems.Select(cartItem => new OrderItem
                {
                    Amount = cartItem.Amount,
                    SinglePrice = cartItem.SinglePrice,
                    ProductID = cartItem.ProductID,
                }).ToList()
            };

            var order = await _dbContext.Orders.AddAsync(newOrder);
            await _dbContext.SaveChangesAsync();
            return order.Entity;
        }
        public async Task<Order> GetOrderById(int orderId)
        {
            return await _dbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
        }
        public async Task<IEnumerable<Order>> GetOrdersWithStatusAsync(OrderStatus orderStatus)
        {
            return await _dbContext.Orders.Where(o => o.OrderStatus == orderStatus).ToListAsync();
        }
        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return null;
            return await _dbContext.Orders.Where(o => o.UserId == userId).ToListAsync();
        }
        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newOrderStatus)
        {
            if (orderId < 1) return false;

            var orderToCancel = await GetOrderById(orderId);
            if (orderToCancel is not Order) return false;

            if (newOrderStatus == OrderStatus.Shipping)
                orderToCancel.TimeConfirmed = DateTime.Now;
            
            orderToCancel.OrderStatus = newOrderStatus;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ConfirmOrderAsync(int orderId)
        {
            return await UpdateOrderStatusAsync(orderId, OrderStatus.Shipping);
        }
        public async Task<bool> CancelOrderAsync(int orderId)
        {
            return await UpdateOrderStatusAsync(orderId, OrderStatus.Cancelled);
        }
    }
}
