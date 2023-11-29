﻿using E_CommerceWebApp.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceWebApp.Services.Repositories
{
    public class ProductRepo : IProductRepo
    {
        ApplicationDBContext _dbContext;
        public ProductRepo(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<Product> GetAllProducts()
        {
            return _dbContext.Products
                    .Include(p => p.ProductImage)
                    .ToList();
        }
        public Product GetProductByID(int id)
        {
            return _dbContext.Products.FirstOrDefault(p => p.ProductID == id);
        }
        public void AddNewProduct(Product product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
        }
    }
}