﻿using MisterS.Web.Models;

namespace MisterS.Web.Services.IServices
{
    public interface IProductService : IBaseService
    {
        Task<T> GetAllProductsAsync<T>(string token);
        Task<T> GetProductByIdAsync<T>(int id, string token);
        Task<T> DeleteProductByIdAsync<T>(int id, string token);
        Task<T> UpdateProductAsync<T>(ProductDto productDto, string token);
        Task<T> CreateProductAsync<T>(ProductDto productDto, string token);
    }
}
