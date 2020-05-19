using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using Entity.Dtos;
using Entity.Dtos.UserDto;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<User> Register(UserRegisterDto registerDto, string password);
        IDataResult<User> Login(UserLoginDto userLoginDto);
        IResult UserExists(string email);
        IDataResult<AccessToken> CreateAccessToken(User user);
        IDataResult<User> UserUpdate(User userToCheck, UserUpdateDto userUpdateDto, string password = null);
        IResult Delete(User user);
        User UserExistsId(int id);
    }
}
 