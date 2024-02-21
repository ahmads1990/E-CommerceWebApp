using E_CommerceWebApp.Models;
using E_CommerceWebApp.Services;
using E_CommerceWebApp.Services.Repositories;
using E_CommerceWebApp.Services.Repositories.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceWebApp.Tests
{
    [TestFixture]
    public class ProductRepoTests
    {
        ApplicationDBContext dbContext;
        IProductRepo productRepo;
        SqliteConnection connection;
        public IEnumerable<Product> GetProductsSeedData()
        {
            return new List<Product>()
            {
                new Product
                {
                    ProductID = 100,
                    ProductName = "product1",
                    ProductImage = null,
                    Price = 10,
                    Description = "product1 Description"
                },
                new Product
                {
                    ProductID = 200,
                    ProductName = "product2",
                    ProductImage = null,
                    Price = 20,
                    Description = "product2 Description"
                }
            };
        }
        [SetUp]
        public void Setup()
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseSqlite(connection)
                .Options;

            // seeding context
            using (var context = new ApplicationDBContext(options))
            {
                context.Database.EnsureCreated();
                context.Products.AddRange(GetProductsSeedData());
                context.SaveChanges();
            }
            // testing context
            dbContext = new ApplicationDBContext(options);
        }
        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
            connection.Close();
        }
        // naming = method name (Testing)_Input State_ Expected Output
        [Test]
        public void GetAllProducts_ValidData_NotNull()
        {
            // Arrange
            productRepo = new ProductRepo(dbContext);
            // Act
            var result = productRepo.GetAllProductsAsync();
            // Assert
            Assert.That(result, Is.Not.Null);
        }
        [Test]
        public async Task GetProductByID_ValidID_ReturnProductAsync()
        {
            int productId = 100; // ensured in seed data
            productRepo = new ProductRepo(dbContext);

            var result = await productRepo.GetProductByIDAsync(productId);

            Assert.That(result.ProductID, Is.EqualTo(productId));
        }
        [Test]
        [TestCase(-100, ExpectedResult = null)]
        [TestCase(0, ExpectedResult = null)]
        public object GetProductByID_InValidID_Null(int id)
        {
            productRepo = new ProductRepo(dbContext);

            return productRepo.GetProductByIDAsync(id);      
        }
        [Test]
        public void AddNewProduct_ValidProduct_Pass()
        {
            productRepo = new ProductRepo(dbContext);
            var product = new Product
            {
                ProductID = 1000,
                ProductName = "product",
                ProductImage = null,
                Price = 10,
                Description = "product Description"
            };

            productRepo.AddNewProductAsync(product);
        }
        [Test]
        public void AddNewProduct_InValidProduct_FailOnNullName()
        {
            productRepo = new ProductRepo(dbContext);
            var product = new Product
            {
                ProductID = 1000,
                ProductName = null,
                ProductImage = null,
                Price = 10,
                Description = "product Description"
            };

            Assert.Throws<DbUpdateException>(() => productRepo.AddNewProductAsync(product));
        }
        [Test]
        public void UpdateProduct_ValidProduct_Pass()
        {
            productRepo = new ProductRepo(dbContext);
            // id from seed
            var product = new Product
            {
                ProductID = 100,
                ProductName = "product",
                ProductImage = null,
                Price = 10,
                Description = "product Description"
            };

            Assert.DoesNotThrow(() => productRepo.UpdateProductAsync(product));
        }
    }
}
