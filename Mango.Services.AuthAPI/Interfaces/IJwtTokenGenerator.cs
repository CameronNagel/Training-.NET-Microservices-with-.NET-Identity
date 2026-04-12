using Mango.Services.AuthAPI.Models;

namespace Mango.Services.AuthAPI.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser user);

    }
}
