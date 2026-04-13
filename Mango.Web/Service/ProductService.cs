using Mango.Web.DTOs;
using Mango.Web.Interfaces;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;

        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> GetAllProductsAsync()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.APIType.GET,
                Url = SD.ProductAPIBase + "/api/productAPI/GetAllProducts",

            });
        }

        public async Task<ResponseDto?> GetProductByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.APIType.GET,
                Url = SD.ProductAPIBase + "/api/productAPI/GetProductById/" + id
            });
        }

        public async Task<ResponseDto?> CreateProductAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.APIType.POST,
                Url = SD.ProductAPIBase + "/api/productAPI/CreateProduct",
                Data = productDto
            });
        }

        public async Task<ResponseDto?> UpdateProductAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.APIType.PUT,
                Url = SD.ProductAPIBase + "/api/productAPI/UpdateProduct",
                Data = productDto
            });
        }

        public async Task<ResponseDto?> DeleteProductAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.APIType.DELETE,
                Url = SD.ProductAPIBase + "/api/productAPI/DeleteProduct/" + id
            });
        }

        public Task<ResponseDto?> GetProductByCategoryAsync(string categoryName)
        {
            return _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.APIType.GET,
                Url = SD.ProductAPIBase + "/api/productAPI/GetProductByCategory/" + categoryName
            });
        }
    }
}
