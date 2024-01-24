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
        // Create
        public async Task<Category> AddNewCategoryAsync(Category category)
        {
            var createdCategory = await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return createdCategory.Entity;
        }
        // Update
        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            var findCategory = await GetCategoryWithIDAsync(category.CategoryID);
            if (findCategory == null)
            {
                return null;
            }
            var updatedCategory = _dbContext.Categories.Update(category);
            await _dbContext.SaveChangesAsync();
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
