using Microsoft.AspNetCore.Mvc;
using MisterS.Services.ShoppingCartAPI.Models.DTO;
using MisterS.Services.ShoppingCartAPI.Repository;

namespace MisterS.Services.ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        protected ResponseDto _responseDto;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
            this._responseDto = new ResponseDto();
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<object> GetCart(string userId)
        {
            try
            {
                var cartDto = await _cartRepository.GetCartByUserId(userId);
                _responseDto.Result = cartDto;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Errors = new List<string>
                {
                    ex.Message
                };
            }
            return _responseDto;
        }

        [HttpPost("AddCart")]
        public async Task<object> AddCart(CartDto cartDto)
        {
            try
            {
                var cart = await _cartRepository.CreateUpdateCart(cartDto);
                _responseDto.Result = cart;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Errors = new List<string>
                {
                    ex.Message
                };
            }
            return _responseDto;
        }

        [HttpPost("UpdateCart")]
        public async Task<object> UpdateCart(CartDto cartDto)
        {
            try
            {
                var cart = await _cartRepository.CreateUpdateCart(cartDto);
                _responseDto.Result = cart;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Errors = new List<string>
                {
                    ex.Message
                };
            }
            return _responseDto;
        }

        [HttpPost("RemoveCart")]
        public async Task<object> RemoveCart([FromBody] int cartId)
        {
            try
            {
                bool isSuccess = await _cartRepository.RemoveFromCart(cartId);
                _responseDto.Result = isSuccess;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Errors = new List<string>
                {
                    ex.Message
                };
            }
            return _responseDto;
        }
    }
}
