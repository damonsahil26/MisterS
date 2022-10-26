using MisterS.Web.Models;
using MisterS.Web.Services.IServices;

namespace MisterS.Web.Services
{
    public class CartService : BaseService, ICartService
    {
        private readonly IHttpClientFactory _httpClient;

        public CartService(IHttpClientFactory httpClient) : base(httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> AddToCartAsync<T>(CartDto cartDto, string? token = null)
        {
            return await this.SendAsync<T>(new ApiRequestModel
            {
                ApiType = Enums.APIType.POST,
                Data = cartDto,
                AccessToken = token ?? "",
                Url = Constants.StaticData.ShoppingCartAPIBaseUri + "api/cart/AddCart"
            });
        }

        public async Task<T> ApplyCouponAsync<T>(CartDto cartDto, string? token = null)
        {
            return await this.SendAsync<T>(new ApiRequestModel
            {
                ApiType = Enums.APIType.POST,
                Data = cartDto,
                AccessToken = token ?? "",
                Url = Constants.StaticData.ShoppingCartAPIBaseUri + "api/cart/ApplyCoupon"
            });
        }

        public async Task<T> CheckOut<T>(CartHeaderDto cartHeaderDto, string? token = null)
        {
            return await this.SendAsync<T>(new ApiRequestModel
            {
                ApiType = Enums.APIType.POST,
                Data = cartHeaderDto,
                AccessToken = token ?? "",
                Url = Constants.StaticData.ShoppingCartAPIBaseUri + "api/cart/CheckOut"
            });
        }

        public async Task<T> GetCartByUserIdAsync<T>(string userId, string? token = null)
        {
            return await this.SendAsync<T>(new ApiRequestModel
            {
                ApiType = Enums.APIType.GET,
                AccessToken = token ?? "",
                Url = Constants.StaticData.ShoppingCartAPIBaseUri + "api/cart/GetCart/" + userId
            });
        }

        public async Task<T> RemoveCouponAsync<T>(string userId, string? token = null)
        {
            return await this.SendAsync<T>(new ApiRequestModel
            {
                ApiType = Enums.APIType.POST,
                Data = userId,
                AccessToken = token ?? "",
                Url = Constants.StaticData.ShoppingCartAPIBaseUri + "api/cart/RemoveCoupon"
            });
        }

        public async Task<T> RemoveFromCartAsync<T>(int cartDetailsId, string? token = null)
        {
            return await this.SendAsync<T>(new ApiRequestModel
            {
                ApiType = Enums.APIType.POST,
                Data = cartDetailsId,
                AccessToken = token ?? "",
                Url = Constants.StaticData.ShoppingCartAPIBaseUri + "api/cart/RemoveCart"
            });
        }

        public async Task<T> UpdateCartAsync<T>(CartDto cartDto, string? token = null)
        {
            return await this.SendAsync<T>(new ApiRequestModel
            {
                ApiType = Enums.APIType.POST,
                Data = cartDto,
                AccessToken = token ?? "",
                Url = Constants.StaticData.ShoppingCartAPIBaseUri + "api/cart/UpdateCart"
            });
        }
    }
}
