using MisterS.Services.ProductAPI.Models.DTO;

namespace MisterS.Services.ProductAPI.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetAll();

        Task<ProductDto> GetById(int productId);

        Task<ProductDto> CreateOrUpdate(ProductDto productDto);

        Task<bool> DeleteProduct(int productId);
    }
}
