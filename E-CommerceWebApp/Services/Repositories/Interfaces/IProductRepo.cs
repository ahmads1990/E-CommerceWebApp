namespace E_CommerceWebApp.Services.Repositories.Interfaces
{
    public interface IProductRepo
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsWithPagination(string searchQuery, int pageNumber, int pageSize);
        int GetProductCount();
        Product GetProductByID(int productID);
        void AddNewProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int productID);
    }
}
