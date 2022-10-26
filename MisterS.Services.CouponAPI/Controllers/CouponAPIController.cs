using Microsoft.AspNetCore.Mvc;
using MisterS.Services.CouponAPI.Models.DTO;
using MisterS.Services.CouponAPI.Repository.IRepository;

namespace MisterS.Services.CouponAPI.Controllers
{
    [ApiController]
    [Route("api/coupon")]
    public class CouponAPIController : Controller
    {
        private readonly ICouponRepository _couponRepository;
        protected ResponseDto responseDto;

        public CouponAPIController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
            responseDto = new ResponseDto();
        }

        [HttpGet("{couponCode}")]
        public async Task<object> GetDiscountForCode(string couponCode)
        {
            try
            {
                var coupon = await _couponRepository.GetCouponFromCouponCode(couponCode);
                responseDto.Result = coupon;
                responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Errors = new List<string>() { ex.Message };
                throw;
            }

            return responseDto;
        }
    }
}
