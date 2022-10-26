using MisterS.Web.Models;
using MisterS.Web.Services.IServices;

namespace MisterS.Web.Services
{
    public class CouponService : BaseService,ICouponService
    {
        private readonly IHttpClientFactory _httpClient;

        public CouponService(IHttpClientFactory httpClient) : base(httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<T> GetCoupon<T>(string couponCode, string? token = null)
        {
            return await this.SendAsync<T>(new ApiRequestModel
            {
                ApiType = Enums.APIType.GET,
                AccessToken = token ?? "",
                Url = Constants.StaticData.CouponAPIBaseUri + "api/coupon/" + couponCode
            });
        }
    }
}
