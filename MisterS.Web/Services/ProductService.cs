using MisterS.Web.Models;
using MisterS.Web.Services.IServices;

namespace MisterS.Web.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> CreateProductAsync<T>(ProductDto productDto, string token)
        {
            return await this.SendAsync<T>(new ApiRequestModel
            {
                ApiType = Enums.APIType.POST,
                Data = productDto,
                AccessToken = token,
                Url = Constants.StaticData.ProductBaseUri + "api/products"
            });
        }

        public async Task<T> DeleteProductByIdAsync<T>(int id, string token)
        {
            return await this.SendAsync<T>(new ApiRequestModel
            {
                ApiType = Enums.APIType.DELETE,
                AccessToken = token,
                Url = Constants.StaticData.ProductBaseUri + "api/products/" + id
            });
        }

        public async Task<T> GetAllProductsAsync<T>(string token)
        {
            return await this.SendAsync<T>(new ApiRequestModel
            {
                ApiType = Enums.APIType.GET,
                AccessToken = token,
                Url = Constants.StaticData.ProductBaseUri + "api/products"
            });
        }

        public async Task<T> GetProductByIdAsync<T>(int id, string token)
        {
            return await this.SendAsync<T>(new ApiRequestModel
            {
                ApiType = Enums.APIType.GET,
                AccessToken = token,
                Url = Constants.StaticData.ProductBaseUri + "api/products/" + id
            });
        }
        public async Task<T> UpdateProductAsync<T>(ProductDto productDto, string token)
        {
            return await this.SendAsync<T>(new ApiRequestModel
            {
                ApiType = Enums.APIType.PUT,
                Data = productDto,
                AccessToken = token,
                Url = Constants.StaticData.ProductBaseUri + "api/products"
            });
        }
    }
}
