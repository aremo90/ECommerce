using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using ECommerce.ServiceAbstractions;
using ECommerce.Shared.DTOS.AuthDTOS;
using ECommerce.Domin.Models.IdentityModule;
using ECommerce.Shared.CommonResult;

namespace ECommerce.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager , IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<bool> CheckEmailAsync(string email)
        {
            var User = await _userManager.FindByEmailAsync(email);
            return User != null;
        }

        public async Task<Result<UserDTO>> GetUserByEmailAsync(string email)
        {
            var User = await _userManager.FindByEmailAsync(email);
            if (User == null)
                return Error.NotFound("User Info Not Found");

            var Token = await GenrateTokenAsync(User);

            return new UserDTO(User.Email!, User.DisplayName, Token);
        }
        public async Task<Result<UserDTO>> LoginAsync(LogInDTO logInDTO)
        {
            var User = await _userManager.FindByEmailAsync(logInDTO.Email); 
            if (User == null)
                return Error.InvalidCredentials("Invalid Credentials Error" , "Invalid Email Or Passowrd !");
            var IsPasswordValid = await _userManager.CheckPasswordAsync(User, logInDTO.Password);
            if (!IsPasswordValid)
                return Error.InvalidCredentials("Invalid Credentials Error", "Invalid Email Or Passowrd !");
            var Token = await GenrateTokenAsync(User);

            return new UserDTO(User.Email!, User.DisplayName, Token);
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
            {
                var Token = await GenrateTokenAsync(User);

                return new UserDTO(User.Email!, User.DisplayName, Token);
            }

            var errors = IdentityResult.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();
            return errors;


        }

        #region JWT Token

        private async Task<string> GenrateTokenAsync(ApplicationUser User)
        {
            // Claims

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, User.Email!),
                new Claim(JwtRegisteredClaimNames.Name, User.UserName!),
            };

            // Roles [Admin , SubAdmin]

            var Roles = await _userManager.GetRolesAsync(User);
            foreach (var role in Roles)
            {
                claims.Add(new Claim("roles", role));
            }


            // Secret Key
            var secretKey = _configuration["JWTOptions:secretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

            // Signing Credentials
            var Cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // create Token
            var Token = new JwtSecurityToken(
                issuer: _configuration["JWTOptions:Issuer"],
                audience: _configuration["JWTOptions:Audience"],
                expires: DateTime.Now.AddHours(2),
                claims: claims,
                signingCredentials: Cred
                );

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }

        #endregion
    }
}
