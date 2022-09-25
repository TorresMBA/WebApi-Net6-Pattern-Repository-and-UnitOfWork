using API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : BaseApiController
    {
        private readonly IUserService _userService;
        public UsuarioController(IUserService userService)
        {
            _userService = userService;
        }
    }
}
