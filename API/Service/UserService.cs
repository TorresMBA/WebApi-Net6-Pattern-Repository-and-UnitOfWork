using API.Dtos;
using API.Helpers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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

        public async Task<DatoUsuarioDto> GetTokenAsync(LoginDto model)
        {
            DatoUsuarioDto datosUsuarioDto = new DatoUsuarioDto();
            var usuario = await _unitOfWork.Usuario
                        .GetByUsernameAsync(model.Username);

            if (usuario == null)
            {
                datosUsuarioDto.EstaAutenticado = false;
                datosUsuarioDto.Mensaje = $"No existe ningún usuario con el username {model.Username}.";
                return datosUsuarioDto;
            }

            var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, model.Password);

            if (resultado == PasswordVerificationResult.Success)
            {
                datosUsuarioDto.EstaAutenticado = true;
                JwtSecurityToken jwtSecurityToken = CreateJwtToken(usuario);
                datosUsuarioDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                datosUsuarioDto.Email = usuario.Email;
                datosUsuarioDto.Username = usuario.Username;
                datosUsuarioDto.Roles = usuario.Roles
                                                .Select(u => u.Nombre)
                                                .ToList();

                if (usuario.RefreshTokens.Any(a => a.IsActive))
                {
                    var activeRefreshToken = usuario.RefreshTokens.Where(a => a.IsActive == true).FirstOrDefault();
                    datosUsuarioDto.RefreshToken = activeRefreshToken.Token;
                    datosUsuarioDto.RefreshTokenExpiration = activeRefreshToken.Expires;
                }
                else
                {
                    var refreshToken = CreateRefreshToken();
                    datosUsuarioDto.RefreshToken = refreshToken.Token;
                    datosUsuarioDto.RefreshTokenExpiration = refreshToken.Expires;
                    usuario.RefreshTokens.Add(refreshToken);
                    _unitOfWork.Usuario.Update(usuario);
                    await _unitOfWork.SaveAsync();
                }

                return datosUsuarioDto;
            }
            datosUsuarioDto.EstaAutenticado = false;
            datosUsuarioDto.Mensaje = $"Credenciales incorrectas para el usuario {usuario.Username}.";
            return datosUsuarioDto;
        }

        public async Task<string> AddRoleAsync(AddRoleDto model)
        {

            var usuario = await _unitOfWork.Usuario
                        .GetByUsernameAsync(model.Username);

            if (usuario == null)
            {
                return $"No existe algún usuario registrado con la cuenta {model.Username}.";
            }

            var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, model.Password);

            if (resultado == PasswordVerificationResult.Success)
            {
                var rolExiste = _unitOfWork.Rol
                                            .Find(u => u.Nombre.ToLower() == model.Role.ToLower())
                                            .FirstOrDefault();

                if (rolExiste != null)
                {
                    var usuarioTieneRol = usuario.Roles
                                                .Any(u => u.Id == rolExiste.Id);

                    if (usuarioTieneRol == false)
                    {
                        usuario.Roles.Add(rolExiste);
                        _unitOfWork.Usuario.Update(usuario);
                        await _unitOfWork.SaveAsync();
                    }

                    return $"Rol {model.Role} agregado a la cuenta {model.Username} de forma exitosa.";
                }

                return $"Rol {model.Role} no encontrado.";
            }
            return $"Credenciales incorrectas para el usuario {usuario.Username}.";
        }

        public async Task<DatoUsuarioDto> RefreshTokenAsync(string refreshToken)
        {
            var datosUsuarioDto = new DatoUsuarioDto();

            var usuario = await _unitOfWork.Usuario
                            .GetByRefreshTokenAsync(refreshToken);

            if (usuario == null)
            {
                datosUsuarioDto.EstaAutenticado = false;
                datosUsuarioDto.Mensaje = $"El token no pertenece a ningún usuario.";
                return datosUsuarioDto;
            }

            var refreshTokenBd = usuario.RefreshTokens.Single(x => x.Token == refreshToken);

            if (!refreshTokenBd.IsActive)
            {
                datosUsuarioDto.EstaAutenticado = false;
                datosUsuarioDto.Mensaje = $"El token no está activo.";
                return datosUsuarioDto;
            }

            //Revocamos el Refresh Token actual y
            refreshTokenBd.Revoked = DateTime.UtcNow;
            //generamos un nuevo Refresh Token y lo guardamos en la Base de Datos
            var newRefreshToken = CreateRefreshToken();
            usuario.RefreshTokens.Add(newRefreshToken);
            _unitOfWork.Usuario.Update(usuario);
            await _unitOfWork.SaveAsync();
            //Generamos un nuevo Json Web Token 😊
            datosUsuarioDto.EstaAutenticado = true;
            JwtSecurityToken jwtSecurityToken = CreateJwtToken(usuario);
            datosUsuarioDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            datosUsuarioDto.Email = usuario.Email;
            datosUsuarioDto.Username = usuario.Username;
            datosUsuarioDto.Roles = usuario.Roles
                                            .Select(u => u.Nombre)
                                            .ToList();
            datosUsuarioDto.RefreshToken = newRefreshToken.Token;
            datosUsuarioDto.RefreshTokenExpiration = newRefreshToken.Expires;

            return datosUsuarioDto;
        }

        private RefreshToken CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomNumber);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomNumber),
                    Expires = DateTime.UtcNow.AddDays(10),
                    Created = DateTime.UtcNow
                };
            }
        }

        private JwtSecurityToken CreateJwtToken(Usuario usuario)
        {
            var roles = usuario.Roles;
            var roleClaims = new List<Claim>();
            
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role.Nombre));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim("uid", usuario.Id.ToString())
            }
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials
            );

            return jwtSecurityToken;
        }
    }
}
