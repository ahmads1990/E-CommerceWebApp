using E_CommerceWebApp.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceWebApp.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductRepo _productRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly ImageService _imageService;
        public ProductsController(IProductRepo productRepo, ImageService imageService, ICategoryRepo categoryRepo)
        {
            _productRepo = productRepo;
            _imageService = imageService;
            _categoryRepo = categoryRepo;
        }

        // GET: Products/ (pageNumber, pageSize)
        public IActionResult Index(int pageNumber = 1, int pageSize = 5)
        {
            int totalRecords = _productRepo.GetProductCount();
            int lastPage = (int)Math.Ceiling((double)totalRecords / pageSize);

            // checking
            if (pageNumber < 1) pageNumber = 1;
            else if (pageNumber > lastPage) pageNumber = lastPage;
            else if (pageSize > totalRecords) pageSize = totalRecords;

            var products = _productRepo.GetProductsWithPagination(null, pageNumber, pageSize);

            return View(new PagedViewModel<IEnumerable<Product>>
                            (products, pageNumber, pageSize, totalRecords, null));
        }
        [HttpGet]
        [AllowAnonymous]
        // GET: Products/DisplayProducts (pageNumber, pageSize)
        // this function is mainly for normal users
        public IActionResult DisplayProducts(string searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            int totalRecords = _productRepo.GetProductCount();
            int lastPage = (int)Math.Ceiling((double)totalRecords / pageSize);

            // checking
            if (pageNumber < 1) pageNumber = 1;
            else if (pageNumber > lastPage) pageNumber = lastPage;
            else if (pageSize > totalRecords) pageSize = totalRecords;

            var products = _productRepo.GetProductsWithPagination(searchQuery, pageNumber, pageSize);

            return View(new PagedViewModel<IEnumerable<Product>>
                            (products, pageNumber, pageSize, totalRecords, searchQuery));
        }
        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            var createProductViewModel = new CreateProductViewModel();
            var categoryList = await _categoryRepo.GetAllCategoriesAsync();

            createProductViewModel.Categories = categoryList.Select(
                c => new SelectListItem { 
                    Text = c.CategoryName, 
                    Value = c.CategoryID.ToString() }
                ).ToList();          

            return View(createProductViewModel);
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
