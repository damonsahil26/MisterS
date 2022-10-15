using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MisterS.Web.Models;
using MisterS.Web.Services.IServices;
using Newtonsoft.Json;

namespace MisterS.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            List<ProductDto> products = new();
            var response = await _productService.GetAllProductsAsync<ResponseDto>(token ?? "");
            if (response != null && response.IsSuccess)
            {
                products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            return View(products);
        }

        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(ProductDto productDto)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            if (ModelState.IsValid)
            {
                var response = await _productService.CreateProductAsync<ResponseDto>(productDto, token ?? "");
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }

            return View(productDto);
        }

        public async Task<IActionResult> EditProduct(int productId)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _productService.GetProductByIdAsync<ResponseDto>(productId, token ?? "");
            if (response != null && response.IsSuccess)
            {
                var product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(product);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var token = await HttpContext.GetTokenAsync("access_token");
                var response = await _productService.UpdateProductAsync<ResponseDto>(productDto, token ?? "");
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }

            return View(productDto);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            var response = await _productService.GetProductByIdAsync<ResponseDto>(productId, token ?? "");
            if (response != null && response.IsSuccess)
            {
                var product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(product);
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var token = await HttpContext.GetTokenAsync("access_token");
                var response = await _productService.DeleteProductByIdAsync<ResponseDto>(productDto.ProductId, token ?? "");
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }

            return View(productDto);
        }


    }
}
