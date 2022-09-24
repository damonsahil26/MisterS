using MisterS.Web.Models;

namespace MisterS.Web.Services.IServices
{
    public interface IProductService : IBaseService
    {
        Task<T> GetAllProductsAsync<T>();
        Task<T> GetProductByIdAsync<T>(int id);
        Task<T> DeleteProductByIdAsync<T>(int id);
        Task<T> UpdateProductAsync<T>(ProductDto productDto);
        Task<T> CreateProductAsync<T>(ProductDto productDto);
    }
}
