using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MisterS.Web.Models;
using MisterS.Web.Services.IServices;
using Newtonsoft.Json;

namespace MisterS.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public CartController(IProductService productService,
            ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartBasedOnLoggedInUser());
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var isSucess = await RemoveItemFromCart(cartDetailsId);
            if (isSucess)
            {
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }

        private async Task<bool> RemoveItemFromCart(int cartDetailsId)
        {
            var userId = User?.Claims?
              .Where(u => u.Type == "sub")?
              .FirstOrDefault()?.Value;

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var isSuccess = false;
            if (userId != null)
            {
                var response = await _cartService.RemoveFromCartAsync<ResponseDto>(cartDetailsId, accessToken);

                return response.IsSuccess;
            }

            return isSuccess;
        }

        private async Task<CartDto> LoadCartBasedOnLoggedInUser()
        {
            var userId = User?.Claims?
                .Where(u => u.Type == "sub")?
                .FirstOrDefault()?.Value;

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var cartDto = new CartDto();
            if (userId != null)
            {
                var response = await _cartService.GetCartByUserIdAsync<ResponseDto>(userId, accessToken);

                if (response != null && response.IsSuccess)
                {
                    cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result) ?? "");
                }

                if (cartDto?.CartHeader != null)
                {
                    foreach (var item in cartDto.CartDetails)
                    {
                        cartDto.CartHeader.OrderTotal += (item.Count * item.Product.Price);
                    }
                }
            }

            return cartDto ?? new CartDto();
        }
    }
}
