using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entity.Dtos.UserDto
{
    public class PasswordResetDto
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public int Id { get; set; }
    }
}
