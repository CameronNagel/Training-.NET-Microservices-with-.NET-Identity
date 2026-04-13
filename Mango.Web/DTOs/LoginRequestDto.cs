using System.ComponentModel.DataAnnotations;

namespace Mango.Web.DTOs
{
    public class LoginRequestDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

    }
}
