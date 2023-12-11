using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace E_CommerceWebApp.Filters
{
    public class UserCartFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;
        
            if (user != null)
            {
                // try to get cart id claim
                var userCartIDClaim = user.FindFirst(CustomClaims.CartId)?.Value;

                int cartId;      
                // if couldn't find the claim or couldn't read the claim value
                if (userCartIDClaim == null || !int.TryParse(userCartIDClaim, out cartId))
                    // Redirect to home
                    context.Result = new RedirectToActionResult("", "Home", null);
                else
                    // pass cart id as a parameter to action
                    context.ActionArguments["cartId"] = cartId;
            }
            base.OnActionExecuting(context);
        }
    }
}
