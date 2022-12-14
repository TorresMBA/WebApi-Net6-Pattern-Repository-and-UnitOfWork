using API.Dtos;

namespace API.Service
{
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterDto registerDto);

        Task<DatoUsuarioDto> GetTokenAsync(LoginDto loginDto);

        Task<string> AddRoleAsync(AddRoleDto addRoleDto);

        Task<DatoUsuarioDto> RefreshTokenAsync(string refreshToken);
    }
}
