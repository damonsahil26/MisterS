using MisterS.Web.Models;
using MisterS.Web.Services.IServices;

namespace MisterS.Web.Services
{
    public class ProductService : IProductService
    {
        public ResponseDto responseModel { get; set; } = new ResponseDto();

        public Task<T> CreateProductAsync<T>(ProductDto productDto)
        {
            throw new NotImplementedException();
        }

        public Task<T> DeleteProductByIdAsync<T>(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAllProductsAsync<T>()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetProductByIdAsync<T>(int id)
        {
            throw new NotImplementedException();
        }

        public Task<T> SendAsync<T>(ApiRequestModel apiRequest)
        {
            throw new NotImplementedException();
        }

        public Task<T> UpdateProductAsync<T>(ProductDto productDto)
        {
            throw new NotImplementedException();
        }
    }
}
