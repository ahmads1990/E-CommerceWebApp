using Microsoft.AspNetCore.Mvc;

namespace E_CommerceWebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepo _cartRepo;

        public CartController(ICartRepo cartRepo)
        {
            _cartRepo = cartRepo;
        }

        public IActionResult Index()
        {
            var items = _cartRepo.GetAllCartItems();
            return View(items);
        }
        public void CreateNewCart()
        {
            _cartRepo.CreateNewCart();
        }
        [HttpPost]
        public void UpdateCartItem(Product product)
        {

            _cartRepo.AddOrUpdateCartItem(product);

        }

    }
}
