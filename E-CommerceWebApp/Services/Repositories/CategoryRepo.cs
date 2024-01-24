using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceWebApp.Services.Repositories
{
    public class CategoryRepo : ICategoryRepo
    {
        ApplicationDBContext _dbContext;
        public CategoryRepo(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        // Read
        public async Task<Category> GetCategoryWithIDAsync(int categoryID)
        {
            return await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryID == categoryID);
        }
        public async Task<Category> GetCategoryWithNameAsync(string categoryName)
        {
            return await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryName == categoryName);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _dbContext.Categories.ToListAsync();
        }
        public Task<IEnumerable<Category>> GetCategoriesWithPaginationAsync(string searchQuery, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> CheckCategoryExistAsync(int id)
        {
            return await _dbContext.Categories.AnyAsync(c => c.CategoryID == id);
        }
        // Create
        public async Task<Category> AddNewCategoryAsync(Category newCategory)
        {
            if (newCategory == null || string.IsNullOrEmpty(newCategory.CategoryName))
                throw new ArgumentException("Invalid category data.");

            if (newCategory.CategoryID != 0)
                throw new ArgumentException("Invalid category ID.");

            var createdCategory = await _dbContext.Categories.AddAsync(newCategory);
            await _dbContext.SaveChangesAsync();

            return createdCategory.Entity;
        }
        // Update
        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            if (category == null || string.IsNullOrEmpty(category.CategoryName))
                throw new ArgumentException("Invalid category data.");

            if (category.CategoryID <= 0)
                throw new ArgumentException("Invalid category ID.");

            if (!await CheckCategoryExistAsync(category.CategoryID)) return null;

            var updatedCategory = _dbContext.Categories.Update(category);
            _dbContext.SaveChanges();

            return updatedCategory.Entity;
        }
        // Delete
        public async Task<Category> DeleteCategoryAsync(int categoryID)
        {
            if (categoryID <= 0)
                throw new ArgumentException("Invalid category ID.");

            var category = await GetCategoryWithIDAsync(categoryID);

            if (category is null) return null;

            var deletedCategory = _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();

            return deletedCategory.Entity;
        }
    }
}
