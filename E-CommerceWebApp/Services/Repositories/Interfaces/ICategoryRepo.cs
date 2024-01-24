namespace E_CommerceWebApp.Services.Repositories.Interfaces
{
    public interface ICategoryRepo
    {
        // Read
        public Task<Category> GetCategoryWithIDAsync(int categoryID);
        public Task<Category> GetCategoryWithNameAsync(string categoryName);
        public Task<IEnumerable<Category>> GetAllCategoriesAsync();
        public Task<IEnumerable<Category>> GetCategoriesWithPaginationAsync(string searchQuery, int pageNumber, int pageSize);
        // Create
        public Task<Category> AddNewCategoryAsync(Category category);
        // Update
        public Task<Category> UpdateCategoryAsync(Category category);
        // Delete
        public Task<Category> DeleteCategoryAsync(int categoryID);
    }
}
