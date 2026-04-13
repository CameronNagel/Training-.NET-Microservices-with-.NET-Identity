using System.IdentityModel.Tokens.Jwt;
using Mango.Web.DTOs;
using Mango.Web.Interfaces;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        [HttpGet] 
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return View(loginRequestDto);
            }

            ResponseDto responseDto = await _authService.LoginAsync(loginRequestDto);

            if (responseDto != null && responseDto.IsSuccess)
            {
                LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));

                await SignInAsync(loginResponseDto);
                _tokenProvider.SetToken(loginResponseDto.Token);
                return RedirectToAction("Index", "Home");
            }


            TempData["error"] = "Login failed: " + responseDto?.Message;
            return View(loginRequestDto);

        }

        [HttpGet] //this gets the registration page and also sends the role list to the view using ViewBag
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem { Text = SD.RoleAdmin, Value = SD.RoleAdmin},
                new SelectListItem { Text = SD.RoleCustomer, Value = SD.RoleCustomer }
            };

            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost] //this handles the registration form submission, calls the registration service, and assigns a role to the user
        public async Task<IActionResult> Register(RegistrationRequestDto registrationRequestDto)
        {
            ResponseDto result = await _authService.RegisterAsync(registrationRequestDto);
            ResponseDto assignRole;

            if (result!=null && result.IsSuccess)
            {
                if (string.IsNullOrEmpty(registrationRequestDto.Role))
                {
                    registrationRequestDto.Role = SD.RoleCustomer;
                }

                assignRole = await _authService.AssignRoleAsync(registrationRequestDto);

                if (assignRole != null && assignRole.IsSuccess)
                {
                    TempData["success"] = "Registration successful!";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["error"] = "Role assignment failed: " + assignRole?.Message;
                }

            }
            else
            {
                TempData["error"] = "Registration failed: " + result?.Message;
            }


            var roleList = new List<SelectListItem>()
            {
                new SelectListItem { Text = SD.RoleAdmin, Value = SD.RoleAdmin},
                new SelectListItem { Text = SD.RoleCustomer, Value = SD.RoleCustomer }
            };

            ViewBag.RoleList = roleList;
            return View(registrationRequestDto);
        }


        [HttpGet]
        public async Task<IActionResult> Logout(LoginRequestDto loginRequestDto)
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index","Home");
        }

        private async Task SignInAsync(LoginResponseDto loginDtoModel)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt  = handler.ReadJwtToken(loginDtoModel.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, 
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
