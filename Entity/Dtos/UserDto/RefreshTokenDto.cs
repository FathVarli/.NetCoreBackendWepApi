using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;

namespace Entity.Dtos.UserDto
{
    public class RefreshTokenDto:IDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
