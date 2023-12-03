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
        public IActionResult UpdateCartItem(int id)
        {
            bool success = true;
            try
            {
                _cartRepo.AddOrUpdateCartItem(id);
            }
            catch (Exception ex)
            {
                success = false;
            }
            return Json(new { success });
        }
    }
}
