using Mango.Web.DTOs;
using Mango.Web.Interfaces;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.APIType.POST,
                Url = SD.AuthAPIBase + "/api/authAPI/Login",
                Data = loginRequestDto
            });
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registerRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.APIType.POST,
                Url = SD.AuthAPIBase + "/api/authAPI/Register",
                Data = registerRequestDto
            });
        }

        public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registerRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.APIType.POST,
                Url = SD.AuthAPIBase + "/api/authAPI/AssignRole",
                Data = registerRequestDto
            });
        }
    }
}
