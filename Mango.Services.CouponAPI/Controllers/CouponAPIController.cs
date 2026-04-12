using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.DTOs;
using Mango.Services.CouponAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController(AppDbContext context) : BaseAPIController
    {

        private readonly ResponseDto response = new();

        [HttpGet("GetAllCoupons")]
        public async Task<ActionResult<IReadOnlyList<ResponseDto>>> GetAllCoupons()
        {
            try
            {
                var coupons = await context.Coupons.ToListAsync();
                response.Result = coupons;
                response.Message = "All Coupons retrieved successfully.";

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error retrieving coupons: {ex.Message}";

            }

            return Ok(response);
        }

        [HttpGet("GetCouponById/{id}")]
        public async Task<ActionResult<ResponseDto>> GetCouponById(int id)
        {
            try
            {
                var coupon = await context.Coupons.FindAsync(id);
                if (coupon == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Coupon not found.";
                    return response;
                }
                response.Result = coupon;
                response.Message = "Coupon retrieved successfully.";

            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = $"Error retrieving coupon: {ex.Message}";
            }

            return response;
        }

        [HttpGet("GetByCode/{code}")]
        public async Task<ActionResult<ResponseDto>> GetCouponByCode(string code)
        {
            var coupon = await context.Coupons.FirstOrDefaultAsync(c => c.CouponCode == code);
            if (coupon == null)
            {
                response.IsSuccess = false;
                response.Message = "Coupon not found.";
                return response;
            }

            response.Result = coupon;
            response.IsSuccess = true;
            response.Message = "Coupon retrieved successfully.";
            return response;
        }

        [HttpPost("CreateCoupon")]
        public async Task<ActionResult<ResponseDto>> CreateCoupon(Coupon coupon)
        {
            try
            {
                context.Coupons.Add(coupon);
                await context.SaveChangesAsync();
                response.Result = coupon;
                response.Message = "Coupon created successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error creating coupon: {ex.Message}";
            }
            return response;
        }

        [HttpPut("UpdateCoupon")]
        public async Task<ActionResult<ResponseDto>> UpdateCoupon(int id, Coupon coupon)
        {
            if (id != coupon.CouponId)
            {
                response.IsSuccess = false;
                response.Message = "Coupon ID mismatch.";
                return response;
            }
            context.Entry(coupon).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
                response.Result = coupon;
                response.Message = "Coupon updated successfully.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.Coupons.Any(e => e.CouponId == id))
                {
                    response.IsSuccess = false;
                    response.Message = "Coupon not found.";
                    return response;
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error updating coupon: {ex.Message}";
            }
            return response;


        }

        [HttpDelete("DeleteCoupon")]
        public async Task<ActionResult<ResponseDto>> DeleteCoupon(int id)
        {
            var coupon = await context.Coupons.FindAsync(id);
            if (coupon == null)
            {
                response.IsSuccess = false;
                response.Message = "Coupon not found.";
                return response;
            }
            context.Coupons.Remove(coupon);
            try
            {
                await context.SaveChangesAsync();
                response.Message = "Coupon deleted successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error deleting coupon: {ex.Message}";
            }
            return response;

        }
    }
}
