using Mango.Web.DTOs;
using Mango.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto> list = new();

            ResponseDto response = await _couponService.GetAllCouponsAsync();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = "ERROR: Coupon(s) could not be retrieved from the server.";
            }

                return View(list);
        }

        public async Task<IActionResult> CouponCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto model)
        {
            if (ModelState.IsValid)
            {
               
                ResponseDto? response = await _couponService.CreateCouponAsync(model);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Coupon created successfully.";
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = "ERROR: Coupon could not be created. -- " + response?.Message;
                }
            }

            return View(model);
        }

        public async Task<IActionResult> CouponDelete(int id)
        {
            ResponseDto? response = await _couponService.GetCouponByIdAsync(id);
            if (response != null && response.IsSuccess)
            {
                CouponDto model = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = "ERROR: Coupon delete not found. -- " + response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto couponDto)
        {
            ResponseDto? response = await _couponService.DeleteCouponAsync(couponDto.CouponId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon deleted successfully.";
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = "ERROR: Coupon could not be deleted. -- " + response?.Message;
            }
            return View(couponDto);
        }

    }
}
