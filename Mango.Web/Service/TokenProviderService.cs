using Mango.Web.Interfaces;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class TokenProviderService : ITokenProvider
    {

        // this IHttpContextAccessor allows us to access the HttpContext of the current request, which can be used to store and retrieve the token in a secure manner, such as using cookies or session storage.
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TokenProviderService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void ClearToken()
        {  
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete(SD.TokenCookie);

        }

        public string? GetToken()
        {
            string? token = null;
            bool? hasToken = _httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue(SD.TokenCookie, out token) ?? false;

            return hasToken == true ? token : null;

        }

        public void SetToken(string token)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(SD.TokenCookie, token);
        }
    }
}
