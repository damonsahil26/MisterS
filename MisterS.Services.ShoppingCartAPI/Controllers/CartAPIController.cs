using Microsoft.AspNetCore.Mvc;
using MisterS.MessageBus.Services;
using MisterS.Services.ShoppingCartAPI.Models.DTO;
using MisterS.Services.ShoppingCartAPI.Repository;

namespace MisterS.Services.ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartAPIController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMessageBusService _messageBusService;
        protected ResponseDto _responseDto;

        public CartAPIController(ICartRepository cartRepository,
            IMessageBusService messageBusService)
        {
            _cartRepository = cartRepository;
            _messageBusService = messageBusService;
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

        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                bool isSuccess = await _cartRepository.ApplyCoupon(cartDto.CartHeader.UserId,
                    cartDto.CartHeader.CouponCode);
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

        [HttpPost("RemoveCoupon")]
        public async Task<object> RemoveCoupon([FromBody] string userId)
        {
            try
            {
                bool isSuccess = await _cartRepository.RemoveCoupon(userId);
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

        [HttpPost("CheckOut")]
        public async Task<object> CheckOut(CheckoutHeaderDto checkoutHeaderDto)
        {
            try
            {
               var cart= await _cartRepository.GetCartByUserId(checkoutHeaderDto.UserId);
                if(cart == null)
                {
                    return BadRequest();
                }
                checkoutHeaderDto.CartDetails = cart.CartDetails;
                //Logic to add message to process order

                await _messageBusService.PublishMessage(checkoutHeaderDto,"checkoutmessagetopic");

                _responseDto.Result = true;
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
