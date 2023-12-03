namespace E_CommerceWebApp.Services.Repositories.Interfaces
{
    public interface IProductRepo
    {
        IEnumerable<Product> GetAllProducts();
        Product GetProductByID(int productID);
        void AddNewProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int productID);
    }
}
