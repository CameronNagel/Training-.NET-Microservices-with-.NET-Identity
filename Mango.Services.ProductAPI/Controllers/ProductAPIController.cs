using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.DTOs;
using Mango.Services.ProductAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductAPIController(AppDbContext context) : BaseAPIController
    {
        private readonly ResponseDto response = new();

        [HttpGet("GetAllProducts")]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<ResponseDto>>> GetAllProducts()
        {
            try
            {
                var products = await context.Products.ToListAsync();
                response.Result = products;
                response.Message = "All Products retrieved successfully.";

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error retrieving products: {ex.Message}";
            }

            return Ok(response);
        }

        [HttpGet("GetProductById/{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto>> GetProductById(int id)
        {
            try
            {
                var product = await context.Products.FindAsync(id);

                if (product == null)
                {
                    response.IsSuccess = false;
                    response.Message = $"Product with ID {id} not found.";
                    return NotFound(response);
                }
                response.Result = product;
                response.Message = $"Product with ID {id} retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error retrieving product with ID {id}: {ex.Message}";
            }

            return Ok(response);
        }

        [HttpGet("GetAllProductsByCategory/{categoryName}")]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<ResponseDto>>> GetAllProductsByCategory(string categoryName)
        {
            try
            {
                var productsByCategory = await context.Products.Where(p => p.CategoryName.ToLower() == categoryName.ToLower()).ToListAsync();
                response.Result = productsByCategory;
                response.Message = $"Products in category '{categoryName}' retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error retrieving products in category '{categoryName}': {ex.Message}";
            }

            return Ok(response);
        }

        [HttpPost("CreateProduct")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ResponseDto>> CreateProduct(Product product)
        {
            try
            {
                context.Products.Add(product);
                await context.SaveChangesAsync();
                response.Result = product;
                response.Message = "Product created successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error creating product: {ex.Message}";
            }
            return Ok(response);
        }

        [HttpPut("UpdateProduct")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ResponseDto>> UpdateProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                response.IsSuccess = false;
                response.Message = "Product ID mismatch.";
                return BadRequest(response);
            }

            context.Entry(product).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
                response.Result = product;
                response.Message = "Product updated successfully.";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!context.Products.Any(p => p.ProductId == id))
                {
                    response.IsSuccess = false;
                    response.Message = $"Product with ID {id} not found.";
                    return NotFound(response);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = $"Error updating product: {ex.Message}";
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
            }
            return Ok(response);
        }

        [HttpDelete("DeleteProduct/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ResponseDto>> DeleteProduct(int id)
        {
            var product = await context.Products.FindAsync(id);
            if (product == null)
            {
                response.IsSuccess = false;
                response.Message = $"Product with ID {id} not found.";
                return NotFound(response);
            }
            context.Products.Remove(product);
            try
            {
                await context.SaveChangesAsync();
                response.Message = "Product deleted successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error deleting product: {ex.Message}";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return Ok(response);
        }
    }
}
