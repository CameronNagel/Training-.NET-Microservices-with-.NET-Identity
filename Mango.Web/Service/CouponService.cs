using Mango.Web.DTOs;
using Mango.Web.Interfaces;
using Mango.Web.Utility;


namespace Mango.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;
        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> CreateCouponAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.APIType.POST,
                Url = SD.CouponAPIBase + "/api/couponAPI/CreateCoupon",
                Data = couponDto
            });
        }

        public async Task<ResponseDto?> DeleteCouponAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.APIType.DELETE,
                Url = SD.CouponAPIBase + "/api/couponAPI/DeleteCoupon/" + id
            });

        }

        public async Task<ResponseDto?> GetAllCouponsAsync()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.APIType.GET,
                Url = SD.CouponAPIBase + "/api/couponAPI/GetAllCoupons"
            });
        }

        public async Task<ResponseDto?> GetCouponByCodeAsync(string couponCode)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.APIType.GET,
                Url = SD.CouponAPIBase + "/api/couponAPI/GetByCode/" + couponCode
            });
        }

        public async Task<ResponseDto?> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.APIType.GET,
                Url = SD.CouponAPIBase + "/api/couponAPI/GetCouponById/" + id
            });
        }

        public async Task<ResponseDto?> UpdateCouponAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.APIType.PUT,
                Url = SD.CouponAPIBase + "/api/couponAPI/UpdateCoupon",
                Data = couponDto
            });

        }
    }
}
