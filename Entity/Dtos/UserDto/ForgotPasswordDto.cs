using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Entity.Dtos.UserDto
{
    public class ForgotPasswordDto:IDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
