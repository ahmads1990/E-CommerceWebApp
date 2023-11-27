using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceWebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepo _productRepo;

        public ProductsController(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }

        // GET: Products
        public IActionResult Index()
        {
            var products = _productRepo.GetAllProducts();
            return View(products);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product) 
        {
            if (ModelState.IsValid)
            {
                // Add new Product
                _productRepo.AddNewProduct(product);
                // return to index
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
    }
}
