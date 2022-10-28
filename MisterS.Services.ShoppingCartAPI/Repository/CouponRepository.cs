using MisterS.Services.ShoppingCartAPI.Models.DTO;
using Newtonsoft.Json;

namespace MisterS.Services.ShoppingCartAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly IHttpClientFactory _client;

        public CouponRepository(IHttpClientFactory client)
        {
            _client = client;
        }

        public async Task<CouponDto> GetCoupon(string couponCode)
        {
            var coupon = new CouponDto();
            var client = _client.CreateClient("MisteSCoupon");
            var response = await client.GetAsync($"api/coupon/{couponCode}");

            var apiContent = await response.Content.ReadAsStringAsync();

            var couponResponse = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if (couponResponse != null && couponResponse.IsSuccess)
            {
                coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(couponResponse.Result));
            }

            return coupon ?? new();
        }
    }
}
