using Core.Entities;

namespace Entity.Dtos.UserDto
{
    public class UserLoginDto:IDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
