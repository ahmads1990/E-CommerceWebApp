using E_CommerceWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_CommerceWebApp.Controllers
{
    [Authorize]
    [UserCartFilter]
    public class CartController : Controller
    {
        private readonly ICartRepo _cartRepo;

        public CartController(ICartRepo cartRepo)
        {
            _cartRepo = cartRepo;
        }

        public IActionResult Index(int cartId)
        {
            // get user cart
            var userCart = _cartRepo.GetCompleteUserCart(cartId);

            return View(userCart);
        }
        // for product card
        [HttpPost]
        public IActionResult UpdateCartItem(int id, int cartId)
        {
            bool success = true;
            try
            {
                _cartRepo.AddOrUpdateCartItem(cartId, id);
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
