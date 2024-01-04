namespace E_CommerceWebApp.Services.Repositories.Interfaces
{
    public interface ICategoryRepo
    {
        public Category GetCategoryWithName(string categoryName);
        public Category GetCategoryWithID(int categoryID);
        public IEnumerable<Category> GetAllCategories();
        public IEnumerable<Category> GetCategoriesWithPagination(string searchQuery, int pageNumber, int pageSize);
        public Category AddNewCategory(Category category);
        public Category UpdateCategory(Category category);
        public Category DeleteCategory(int categoryID);
    }
}
