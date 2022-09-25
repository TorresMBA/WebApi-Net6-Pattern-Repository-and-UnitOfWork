using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class AddRoleDto
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
    }
}
