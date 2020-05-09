using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;

namespace Entity.Dtos
{
    public class UserRegisterDto:IDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
