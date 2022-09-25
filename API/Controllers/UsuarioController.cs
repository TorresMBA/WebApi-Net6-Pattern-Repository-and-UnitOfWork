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
    }
}
