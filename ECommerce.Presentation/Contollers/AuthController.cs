using ECommerce.ServiceAbstractions;
using ECommerce.Shared.DTOS.AuthDTOS;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Contollers
{
    public class AuthController : ApiBaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        #region Login
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LogInDTO logInDTO)
        {
            var result = await _authService.LoginAsync(logInDTO);
            return HandleRequest(result);
        }
        #endregion
        #region Login
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            var result = await _authService.RegisterAsync(registerDTO);
            return HandleRequest(result);
        }
        #endregion
    }
}
