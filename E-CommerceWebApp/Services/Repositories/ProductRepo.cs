using E_CommerceWebApp.Services.Repositories.Interfaces;
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
        public IEnumerable<Product> GetAllProducts()
        {
            return _dbContext.Products
                    .Include(p => p.ProductImage)
                    .ToList();
        }
        public IEnumerable<Product> GetProductsWithPagination(int pageNumber, int pageSize)
        {
            int startingIndex = (pageNumber - 1) * pageSize;

            return _dbContext.Products
                    .Include(p => p.ProductImage)
                    .OrderBy(p => p.ProductID)
                    .Skip(startingIndex)
                    .Take(pageSize)
                    .ToList();
        }
        public int GetProductCount()
        {
            return _dbContext.Products.Count();
        }
        public Product GetProductByID(int productID)
        {
            return _dbContext.Products.FirstOrDefault(p => p.ProductID == productID);
        }
        public void AddNewProduct(Product product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
        }
        public void UpdateProduct(Product product)
        {
            _dbContext.Products.Update(product);
            _dbContext.SaveChanges();
        }
        public void DeleteProduct(int productID)
        {
            var existingProduct = GetProductByID(productID);
            if (existingProduct != null)
            {
                _dbContext.Products.Remove(existingProduct);
                _dbContext.SaveChanges();
            }
        }
    }
}
