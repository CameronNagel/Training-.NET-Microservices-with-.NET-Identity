using Mango.Services.AuthAPI.DTOs;

namespace Mango.Services.AuthAPI.Interfaces
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registerRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    }
}
