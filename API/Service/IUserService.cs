using API.Dtos;

namespace API.Service
{
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterDto registerDto);
    }
}
