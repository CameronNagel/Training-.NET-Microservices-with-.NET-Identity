using Mango.Web.DTOs;

namespace Mango.Web.Interfaces
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAsync(RequestDto requestDto);
    }
}
