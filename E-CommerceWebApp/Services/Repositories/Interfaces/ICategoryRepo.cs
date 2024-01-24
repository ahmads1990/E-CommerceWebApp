namespace E_CommerceWebApp.Services.Repositories.Interfaces
{
    public interface ICategoryRepo
    {
        // Read
        public Category GetCategoryWithID(int categoryID);
        public Category GetCategoryWithName(string categoryName);
        public IEnumerable<Category> GetAllCategories();
        public IEnumerable<Category> GetCategoriesWithPagination(string searchQuery, int pageNumber, int pageSize);
        // Create
        public Category AddNewCategory(Category category);
        // Update
        public Category UpdateCategory(Category category);
        // Delete
        public Category DeleteCategory(int categoryID);
    }
}
