using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_CommerceWebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepo _cartRepo;

        public CartController(ICartRepo cartRepo)
        {
            _cartRepo = cartRepo;
        }

        [Authorize]
        public IActionResult Index()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            string userId = (userIdClaim != null) ? (string)userIdClaim.Value : null;

            // check if user doesn't have a cart
            if (_cartRepo.GetUserCart(userId) == null) RedirectToAction("", "Home");
            // get user cart
            var userCart = _cartRepo.GetCompleteUserCart(userId);

            return View(userCart);
        }
        // for product card
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
        // update
        public IActionResult UpdateCartItemAmount(int itemID, int amount)
        {
            try
            {
                _cartRepo.UpdateCartItemAmount(itemID, amount);
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction(nameof(Index));
        }
        // delete
        public IActionResult RemoveCartItem(int id)
        {
            _cartRepo.RemoveCartItem(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
