using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MisterS.Services.ProductAPI.DbContexts;
using MisterS.Services.ProductAPI.Models;
using MisterS.Services.ProductAPI.Models.DTO;

namespace MisterS.Services.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ProductDto> CreateOrUpdate(ProductDto productDto)
        {
            var product = _mapper.Map<ProductDto, Product>(productDto);

            if (product.ProductId > 0)
            {
                _dbContext.Products.Update(product);
            }
            else
            {
                _dbContext.Products.Add(product);
            }

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<Product, ProductDto>(product);
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
                if (product == null)
                {
                    return false;
                }
                else
                {
                    _dbContext.Products.Remove(product);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            var products = await _dbContext.Products.ToListAsync();

            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto> GetById(int productId)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
            if (product == null)
            {
                return new();
            }

            return _mapper.Map<Product, ProductDto>(product);
        }
    }
}
