using API.Dtos;
using API.Helpers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Service
{
    public class UserService : IUserService
    {
        private readonly JWT _jwt;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public UserService(IUnitOfWork unitOfWork, IOptions<JWT> jwt, IPasswordHasher<Usuario> passwordHasher )
        {
            _jwt = jwt.Value;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            var usuario = new Usuario()
            {
                Nombres = registerDto.Nombres,
                ApellidoPaterno = registerDto.ApellidoPaterno,
                ApellidoMaterno= registerDto.ApellidoMaterno,
                Email = registerDto.Email,
                Username = registerDto.Username,
            };

            usuario.Password = _passwordHasher.HashPassword(usuario, registerDto.Password);

            var usuarioExiste = _unitOfWork.Usuario
                                .Find(u => u.Username.ToLower() == registerDto.Username.ToLower())
                                .FirstOrDefault();

            if (usuarioExiste == null)
            {
                var rolPredeterminado = _unitOfWork.Rol
                                                    .Find(u => u.Nombre == Autorizacion.rol_predeterminado.ToString())
                                                    .First();

                try
                {
                    usuario.Roles.Add(rolPredeterminado);
                    _unitOfWork.Usuario.Add(usuario);
                    await _unitOfWork.SaveAsync();

                    return $"El usuario {registerDto.Username} ha sido registrado correctamente.";
                }
                catch (Exception ex)
                {
                    return $"Error: {ex.Message}";
                }
            }
            else
            {
                return $"El usuario con {registerDto.Username} ya se encuentra registrado";
            }
        }
    }
}
