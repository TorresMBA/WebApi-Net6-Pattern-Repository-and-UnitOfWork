using System.Text.Json.Serialization;

namespace API.Dtos
{
    public class DatoUsuarioDto
    {
        public string Mensaje { get; set; }

        public bool EstaAutenticado { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public List<string> Roles { get; set; }

        public string Token { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiration { get; set; }
    }
}
