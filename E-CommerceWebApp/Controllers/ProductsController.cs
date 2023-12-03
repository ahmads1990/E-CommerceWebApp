using E_CommerceWebApp.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceWebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepo _productRepo;
        private readonly ImageService _imageService;
        public ProductsController(IProductRepo productRepo, ImageService imageService)
        {
            _productRepo = productRepo;
            _imageService = imageService;
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
        public IActionResult Create(CreateProductViewModel createProductViewModel) 
        {
            if (ModelState.IsValid)
            {
                if(!_imageService.CheckImage(createProductViewModel.ProductImage)) 
                {
                    //return error
                    return View(createProductViewModel);
                }

                // Add new Product
                var newProduct = createProductViewModel.toProduct();
                _productRepo.AddNewProduct(newProduct);
                // return to index
                return RedirectToAction(nameof(Index));
            }
            return View(createProductViewModel);
        }
        // GET: Products/Details/5
        public IActionResult Details(int productID)
        {
            var product = _productRepo.GetProductByID(productID);

            return View(product);
        }
        [HttpGet]
        public IActionResult Update(int productID)
        {
            var product = _productRepo.GetProductByID(productID);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        // Post: Products/Update/5
        [HttpPost]
        public IActionResult Update(Product product)
        {
            try
            {
                _productRepo.UpdateProduct(product);

            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        // Post: Products/Delete/5
        public  IActionResult Delete(int productID)
        {
            _productRepo.DeleteProduct(productID); 
            return RedirectToAction(nameof(Index));
        }
    }
}
