using E_CommerceWebApp.Models;
using Microsoft.CodeAnalysis;

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
        public Category GetCategoryWithID(int categoryID)
        {
            return _dbContext.Categories.FirstOrDefault(c => c.CategoryID == categoryID);
        }
        public Category GetCategoryWithName(string categoryName)
        {
            return _dbContext.Categories.FirstOrDefault(c => c.CategoryName == categoryName);
        }
      
        public IEnumerable<Category> GetAllCategories()
        {
            return _dbContext.Categories.ToList();
        }
        public IEnumerable<Category> GetCategoriesWithPagination(string searchQuery, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }
        // Create
        public Category AddNewCategory(Category category)
        {
            var createdCategory = _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();
            return createdCategory.Entity;
        }
        // Update
        public Category UpdateCategory(Category category)
        {
            var updatedCategory = _dbContext.Categories.Update(category);
            _dbContext.SaveChanges();
            return updatedCategory.Entity;
        }
        // Delete
        public Category DeleteCategory(int categoryID)
        {
            var existingCategory = GetCategoryWithID(categoryID);
            if (existingCategory != null)
            {
                _dbContext.Categories.Remove(existingCategory);
                _dbContext.SaveChanges();
            }
            return existingCategory;
        }

    }
}
