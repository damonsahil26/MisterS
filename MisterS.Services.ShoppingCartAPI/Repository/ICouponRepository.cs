using MisterS.Services.ShoppingCartAPI.Models.DTO;

namespace MisterS.Services.ShoppingCartAPI.Repository
{
    public interface ICouponRepository
    {
        public Task<CouponDto> GetCoupon(string couponCode);
    }
}
