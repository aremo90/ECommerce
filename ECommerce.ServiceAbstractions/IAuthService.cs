using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOS.AuthDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServiceAbstractions
{
    public interface IAuthService
    {
        // login
        Task<Result<UserDTO>> LoginAsync(LogInDTO logInDTO);
        // register
        Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO);
        Task<bool> CheckEmailAsync(string email);
        Task<Result<UserDTO>> GetUserByEmailAsync(string email);
    }
}
