namespace MisterS.Web.Services.IServices
{
    public interface ICouponService
    {
        public Task<T> GetCoupon<T>(string couponId, string? token = null);
    }
}
