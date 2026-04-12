using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.DTOs;
using Mango.Services.AuthAPI.Interfaces;
using Mango.Services.AuthAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(AppDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _dbContext.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (user == null || !isValid)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }

            UserDto userDto = new UserDto
            {
                Id = Guid.Parse(user.Id),
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDto loginResponseDto = new LoginResponseDto
            {
                User = userDto,
                Token = ""

            };

            return loginResponseDto;

        }

        public async Task<string> Register(RegistrationRequestDto registerRequestDto)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = registerRequestDto.Email,
                Email = registerRequestDto.Email,
                NormalizedEmail = registerRequestDto.Email.ToUpper(),
                Name = registerRequestDto.Name,
                PhoneNumber = registerRequestDto.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registerRequestDto.Password);
                if(result.Succeeded)
                {
                    var userToReturn = _dbContext.ApplicationUsers.First(u => u.UserName == registerRequestDto.Email);
                    
                    UserDto userDto = new UserDto
                    {
                        Id = Guid.Parse(userToReturn.Id),
                        Name = userToReturn.Name,
                        Email = userToReturn.Email,
                        PhoneNumber = userToReturn.PhoneNumber
                    };

                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {
                
            }
            return "Error Encounted";
        }
    }
}
