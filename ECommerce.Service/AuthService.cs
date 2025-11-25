using ECommerce.Domin.Models.IdentityModule;
using ECommerce.ServiceAbstractions;
using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOS.AuthDTOS;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<UserDTO>> LoginAsync(LogInDTO logInDTO)
        {
            var User = await _userManager.FindByEmailAsync(logInDTO.Email); 
            if (User == null)
                return Error.InvalidCredentials("Invalid Credentials Error" , "Invalid Email Or Passowrd !");
            var IsPasswordValid = await _userManager.CheckPasswordAsync(User, logInDTO.Password);
            if (!IsPasswordValid)
                return Error.InvalidCredentials("Invalid Credentials Error", "Invalid Email Or Passowrd !");

            return new UserDTO(User.Email!, User.DisplayName, "Token");
        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            var User = new ApplicationUser()
            {
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName,
                UserName = registerDTO.UserName,
                PhoneNumber = registerDTO.PhoneNumber
            };

            var IdentityResult = await  _userManager.CreateAsync(User, registerDTO.Password);

            if (IdentityResult.Succeeded)
                return new UserDTO(User.Email!, User.DisplayName, "Token");

            var errors = IdentityResult.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();
            return errors;


        }
    }
}
