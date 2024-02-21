using Microsoft.EntityFrameworkCore;

namespace E_CommerceWebApp.Services.Repositories
{
    public class ProductRepo : IProductRepo
    {
        ApplicationDBContext _dbContext;
        public ProductRepo(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _dbContext.Products
                        .Include(p => p.ProductImage)
                        .ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProductsWithPaginationAsync(string searchQuery, int pageNumber, int pageSize)
        {
            // start the query
            var query = _dbContext.Products.AsQueryable();
            // apply search by name query if existed
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(p => p.ProductName.Contains(searchQuery));
            }
            // paginate and return the result
            int startingIndex = (pageNumber - 1) * pageSize;
            if (startingIndex < 0) return new List<Product>();
            return await query
                        .Include(p => p.ProductImage)
                        .OrderBy(p => p.ProductID)
                        .Skip(startingIndex)
                        .Take(pageSize)
                        .ToListAsync();
        }
        public async Task<int> GetProductCountAsync()
        {
            return await _dbContext.Products.CountAsync();
        }
        public async Task<Product> GetProductByIDAsync(int productID)
        {
            return await _dbContext.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.ProductID == productID);
        }
        public async Task AddNewProductAsync(Product product)
        {
            _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateProductAsync(Product product)
        {
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteProductAsync(int productID)
        {
            var existingProduct = await GetProductByIDAsync(productID);
            if (existingProduct != null)
            {
                _dbContext.Products.Remove(existingProduct);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
