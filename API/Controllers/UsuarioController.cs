using API.Dtos;
using API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UsuarioController : BaseApiController
    {
        private readonly IUserService _userService;

        public UsuarioController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync(RegisterDto registerDto)
        {
            var result = await _userService.RegisterAsync(registerDto);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult> GetTokenAsync(LoginDto loginDto)
        {
            var result = await _userService.GetTokenAsync(loginDto);

            return Ok(result);
        }

        [HttpPost("addrole")]
        public async Task<ActionResult> AddRoleAsync(AddRoleDto addRoleDto)
        {
            var result = await _userService.AddRoleAsync(addRoleDto);

            return Ok(result);
        }
    }
}
