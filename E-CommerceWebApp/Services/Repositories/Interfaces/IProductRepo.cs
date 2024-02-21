namespace E_CommerceWebApp.Services.Repositories.Interfaces
{
    public interface IProductRepo
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetProductsWithPaginationAsync(string searchQuery, int pageNumber, int pageSize);
        Task<int> GetProductCountAsync();
        Task<Product> GetProductByIDAsync(int productID);
        Task AddNewProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int productID);
    }
}
