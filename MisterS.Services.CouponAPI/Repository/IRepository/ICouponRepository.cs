using MisterS.Services.CouponAPI.Models.DTO;

namespace MisterS.Services.CouponAPI.Repository.IRepository
{
    public interface ICouponRepository
    {
        Task<CouponDto> GetCouponFromCouponCode(string couponCode);
    }
}
