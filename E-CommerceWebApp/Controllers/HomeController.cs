using E_CommerceWebApp.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace E_CommerceWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepo _productRepo;
        private const int defaultProductAmount = 20;
        public HomeController(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepo.GetProductsWithPaginationAsync(null, 1, defaultProductAmount);
            var viewModel = new HomeViewModel { Products = products };
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}