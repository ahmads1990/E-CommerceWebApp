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
    }
}
