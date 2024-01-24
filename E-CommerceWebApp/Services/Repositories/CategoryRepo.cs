using E_CommerceWebApp.Models;
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

        public IEnumerable<Category> GetAllCategories()
        {
            return _dbContext.Categories.ToList();
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
            var existingCategory = await GetCategoryWithIDAsync(categoryID);
            if (existingCategory != null)
            {
                _dbContext.Categories.Remove(existingCategory);
                await _dbContext.SaveChangesAsync();
            }
            return existingCategory;
        }
    }
}
