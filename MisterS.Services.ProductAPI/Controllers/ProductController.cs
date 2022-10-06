using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MisterS.Services.ProductAPI.Models.DTO;
using MisterS.Services.ProductAPI.Repository;

namespace MisterS.Services.ProductAPI.Controllers
{
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        protected ResponseDto _responseDto;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _responseDto = new();
        }

        [HttpGet]
        [Authorize]
        public async Task<ResponseDto> GetProducts()
        {
            try
            {
                var products = await _productRepository.GetAll();
                _responseDto.Result = products;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Errors = new List<string>
                {
                    ex.ToString()
                };
            }
            return _responseDto;
        }

        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public async Task<ResponseDto> GetProductById(int id)
        {
            try
            {
                var product = await _productRepository.GetById(id);
                _responseDto.Result = product;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Errors = new List<string>
                {
                    ex.ToString()
                };
            }
            return _responseDto;
        }

        [HttpPost]
        [Authorize]
        public async Task<ResponseDto> CreateProduct([FromBody] ProductDto productDto)
        {
            try
            {
                var product = await _productRepository.CreateOrUpdate(productDto);
                _responseDto.Result = product;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Errors = new List<string>
                {
                    ex.ToString()
                };
            }
            return _responseDto;
        }

        [HttpPut]
        [Authorize]
        public async Task<ResponseDto> UpdateProduct([FromBody] ProductDto productDto)
        {
            try
            {
                var product = await _productRepository.CreateOrUpdate(productDto);
                _responseDto.Result = product;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Errors = new List<string>
                {
                    ex.ToString()
                };
            }
            return _responseDto;
        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        [Route("{id}")]
        public async Task<ResponseDto> DeleteProduct(int id)
        {
            try
            {
                var isSuccess = await _productRepository.DeleteProduct(id);
                _responseDto.IsSuccess = isSuccess;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Errors = new List<string>
                {
                    ex.ToString()
                };
            }
            return _responseDto;
        }
    }
}
