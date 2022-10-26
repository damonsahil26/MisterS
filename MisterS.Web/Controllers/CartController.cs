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
        private readonly ICouponService _couponService;

        public CartController(IProductService productService,
            ICartService cartService,
            ICouponService couponService)
        {
            _productService = productService;
            _cartService = cartService;
            _couponService = couponService;
        }

        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartBasedOnLoggedInUser());
        }

        [HttpPost]
        [ActionName("ApplyCoupon")]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            var userId = User?.Claims?
            .Where(u => u.Type == "sub")?
            .FirstOrDefault()?.Value;

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            if (userId != null)
            {
                var response = await _cartService.ApplyCouponAsync<ResponseDto>(cartDto, accessToken);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(CartIndex));
                }
            }

            return View();
        }


        [HttpPost]
        [ActionName("RemoveCoupon")]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            var userId = User?.Claims?
            .Where(u => u.Type == "sub")?
            .FirstOrDefault()?.Value;

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            if (userId != null)
            {
                var response = await _cartService.RemoveCouponAsync<ResponseDto>(cartDto.CartHeader.UserId, accessToken);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(CartIndex));
                }
            }

            return View();
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
                    if (!string.IsNullOrEmpty(cartDto?.CartHeader?.CouponCode))
                    {
                        var couponResponse = await _couponService.GetCoupon<ResponseDto>(cartDto?.CartHeader?.CouponCode, accessToken);

                        if (couponResponse != null && couponResponse.IsSuccess)
                        {

                            var coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(couponResponse.Result));
                            if (coupon != null)
                            {
                                cartDto.CartHeader.DiscountTotal = coupon.DiscountAmount;
                            }
                        }
                    }

                    foreach (var item in cartDto.CartDetails)
                    {
                        cartDto.CartHeader.OrderTotal += (item.Count * item.Product.Price);
                    }

                    cartDto.CartHeader.OrderTotal -= cartDto.CartHeader.DiscountTotal;
                }
            }

            return cartDto ?? new CartDto();
        }

        public async Task<IActionResult> CheckOut()
        {
            return View(await LoadCartBasedOnLoggedInUser());
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(CartDto cartDto)
        {
            try
            {
                var access_token = await HttpContext.GetTokenAsync("access_token");
                var checkoutResponse = await _cartService.CheckOut<ResponseDto>(cartDto?.CartHeader, access_token);
                if (checkoutResponse != null && checkoutResponse.IsSuccess)
                {

                }
                return RedirectToAction(nameof(Confirmation));
            }
            catch (Exception)
            {

                return View(cartDto); ;
            }
        }

        public async Task<IActionResult> Confirmation()
        {
            try
            {

                return View();

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
