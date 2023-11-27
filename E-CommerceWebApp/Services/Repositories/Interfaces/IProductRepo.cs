namespace E_CommerceWebApp.Services.Repositories.Interfaces
{
    public interface IProductRepo
    {
        IEnumerable<Product> GetAllProducts();
        Product GetProductByID(int id);

        void AddNewProduct(Product product);
    }
}
