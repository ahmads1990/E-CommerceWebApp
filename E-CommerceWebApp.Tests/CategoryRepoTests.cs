using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using E_CommerceWebApp.Models;
using E_CommerceWebApp.Services.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_CommerceWebApp.Services.Repositories.Interfaces;
using E_CommerceWebApp.Services;

namespace E_CommerceWebApp.Tests
{
    [TestFixture]
    public class CategoryRepoTests
    {
        ApplicationDBContext appDbContext;
        ICategoryRepo categoryRepo;
        SqliteConnection connection;
        static int seedDataCount = 5;
        static int nonExistingId = 1000;

        public IEnumerable<Category> GetCategoriesSeedData()
        {
            return new List<Category>()
            {
                new Category { CategoryID = 1, CategoryName = "Category1" },
                new Category { CategoryID = 2, CategoryName = "Category2" },
                new Category { CategoryID = 3, CategoryName = "Category3" },
                new Category { CategoryID = 4, CategoryName = "Category4" },
                new Category { CategoryID = 5, CategoryName = "Category5" },
            };
        }

        [SetUp]
        public void Setup()
        {
            // create and open new SQLite Connection
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            // configure options 
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
               .UseSqlite(connection)
               .Options;
            // seeding new context 
            using (var context = new ApplicationDBContext(options))
            {
                context.Database.EnsureCreated();
                context.Categories.AddRange(GetCategoriesSeedData());
                context.SaveChanges();
            }
            // testing context
            appDbContext = new ApplicationDBContext(options);
        }

        [TearDown]
        public void TearDown()
        {
            appDbContext.Dispose();
            connection.Close();
        }

        // GetByIdAsync
        [Test]
        public async Task GetCategoryWithIDAsync_ValidId_ValidCategory()
        {
            // Arrange
            categoryRepo = new CategoryRepo(appDbContext);
            int testId = 1;
            var expectedCategory = GetCategoriesSeedData().FirstOrDefault(c => c.CategoryID == testId);
            // Act
            var result = await categoryRepo.GetCategoryWithIDAsync(testId);
            // Assert
            Assert.That(result.CategoryID, Is.EqualTo(expectedCategory.CategoryID));
        }

        [Test]
        public async Task GetCategoryWithIDAsync_InvalidId_Null()
        {
            // Arrange
            categoryRepo = new CategoryRepo(appDbContext);
            int testId = -1;
            // Act
            var result = await categoryRepo.GetCategoryWithIDAsync(testId);
            // Assert
            Assert.That(result, Is.Null);
        }

        // GetCategoryWithNameAsync
        [Test]
        public async Task GetCategoryWithNameAsync_ValidName_ValidCategory()
        {
            // Arrange
            categoryRepo = new CategoryRepo(appDbContext);
            string testName = "Category1";
            var expectedCategory = GetCategoriesSeedData().FirstOrDefault(c => c.CategoryName == testName);
            // Act
            var result = await categoryRepo.GetCategoryWithNameAsync(testName);
            // Assert
            Assert.That(result.CategoryID, Is.EqualTo(expectedCategory.CategoryID));
        }

        [Test]
        public async Task GetCategoryWithNameAsync_InvalidName_Null()
        {
            // Arrange
            categoryRepo = new CategoryRepo(appDbContext);
            string testName = "NonExistingCategory";
            // Act
            var result = await categoryRepo.GetCategoryWithNameAsync(testName);
            // Assert
            Assert.That(result, Is.Null);
        }

        // GetAllCategoriesAsync
        [Test]
        public async Task GetAllCategoriesAsync_ValidData_NotNull()
        {
            // Arrange
            categoryRepo = new CategoryRepo(appDbContext);
            // Act
            var result = await categoryRepo.GetAllCategoriesAsync();
            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetAllCategoriesAsync_ValidData_CountEqualSeed()
        {
            // Arrange
            categoryRepo = new CategoryRepo(appDbContext);
            // Act
            var result = await categoryRepo.GetAllCategoriesAsync();
            // Assert
            Assert.That(result.Count(), Is.EqualTo(seedDataCount));
        }

        // AddNewCategoryAsync
        [Test]
        public async Task AddNewCategoryAsync_ValidCategory_NewCategory()
        {
            // Arrange
            categoryRepo = new CategoryRepo(appDbContext);
            var newCategory = new Category { CategoryName = "NewCategory" };
            // Act
            var result = await categoryRepo.AddNewCategoryAsync(newCategory);
            // Assert
            Assert.That(result.CategoryName, Is.EqualTo(newCategory.CategoryName));
        }

        [Test]
        public async Task AddNewCategoryAsync_InvalidCategoryName_Throws()
        {
            // Arrange
            categoryRepo = new CategoryRepo(appDbContext);
            var newCategory = new Category { CategoryName = "" };

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await categoryRepo.AddNewCategoryAsync(newCategory));

            Assert.That(exception.Message, Is.EqualTo("Invalid category data."));
        }

        [Test]
        public async Task AddNewCategoryAsync_InvalidFormatCategoryId_Throws()
        {
            // Arrange
            categoryRepo = new CategoryRepo(appDbContext);
            var newCategory = new Category { CategoryID = -1, CategoryName = "CategoryName" };

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await categoryRepo.AddNewCategoryAsync(newCategory));

            Assert.That(exception.Message, Is.EqualTo("Invalid category ID."));
        }

        // UpdateCategoryAsync
        [Test]
        public async Task UpdateCategoryAsync_ValidCategory_UpdatedCategory()
        {
            // Arrange
            categoryRepo = new CategoryRepo(appDbContext);
            var updatedCategory = GetCategoriesSeedData().First();
            updatedCategory.CategoryName = "UpdatedCategory";
            // Act
            var result = await categoryRepo.UpdateCategoryAsync(updatedCategory);
            // Assert
            Assert.That(result.CategoryID, Is.EqualTo(updatedCategory.CategoryID));
            Assert.That(result.CategoryName, Is.EqualTo(updatedCategory.CategoryName));
        }

        [Test]
        public async Task UpdateCategoryAsync_InvalidCategoryName_Throws()
        {
            // Arrange
            categoryRepo = new CategoryRepo(appDbContext);
            var updatedCategory = GetCategoriesSeedData().First();
            updatedCategory.CategoryName = null;

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await categoryRepo.UpdateCategoryAsync(updatedCategory));

            Assert.That(exception.Message, Is.EqualTo("Invalid category data."));
        }

        [Test]
        public async Task UpdateCategoryAsync_InvalidFormatCategoryId_Throws()
        {
            // Arrange
            categoryRepo = new CategoryRepo(appDbContext);
            var toUpdateCategory = new Category { CategoryID = -1, CategoryName = "CategoryName" };

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await categoryRepo.UpdateCategoryAsync(toUpdateCategory));

            Assert.That(exception.Message, Is.EqualTo("Invalid category ID."));
        }

        [Test]
        public async Task UpdateCategoryAsync_NonExistingCategoryId_Throws()
        {
            // Arrange
            categoryRepo = new CategoryRepo(appDbContext);
            var toUpdateCategory = new Category { CategoryID = nonExistingId, CategoryName = "CategoryName" };

            var result = await categoryRepo.UpdateCategoryAsync(toUpdateCategory);

            Assert.That(result, Is.Null);
        }

        // DeleteCategoryAsync
        [Test]
        public async Task DeleteCategoryAsync_ValidId_toDeleteCategory()
        {
            // Arrange
            categoryRepo = new CategoryRepo(appDbContext);
            var toDeleteCategory = GetCategoriesSeedData().First();
            // Act
            var result = await categoryRepo.DeleteCategoryAsync(toDeleteCategory.CategoryID);
            // Assert
            Assert.That(result.CategoryID, Is.EqualTo(toDeleteCategory.CategoryID));
        }

        [Test]
        public async Task DeleteCategoryAsync_InvalidFormatCategoryId_Throws()
        {
            // Arrange
            categoryRepo = new CategoryRepo(appDbContext);
            var toDeleteCategory = new Category { CategoryID = -1, CategoryName = "CategoryName" };

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await categoryRepo.DeleteCategoryAsync(toDeleteCategory.CategoryID));

            Assert.That(exception.Message, Is.EqualTo("Invalid category ID."));
        }

        [Test]
        public async Task DeleteCategoryAsync_NonExistingCategoryId_Throws()
        {
            // Arrange
            categoryRepo = new CategoryRepo(appDbContext);
            var toDeleteCategory = new Category { CategoryID = nonExistingId, CategoryName = "CategoryName" };

            var result = await categoryRepo.DeleteCategoryAsync(toDeleteCategory.CategoryID);

            Assert.That(result, Is.Null);
        }
    }
}
