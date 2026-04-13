using Mango.Web.DTOs;

namespace Mango.Web.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registerRequestDto);
        Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registerRequestDto);


    }
}
