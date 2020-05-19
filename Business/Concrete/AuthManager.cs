using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using Entity.Dtos;
using Entity.Dtos.UserDto;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.IdentityModel.Tokens;
using TurkishCitizenIdValidator;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;
        private IUserDal _userDal;
        private IRefreshTokenDal _refreshTokenDal;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper, IUserDal userDal, IRefreshTokenDal refreshTokenDal)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _userDal = userDal;
            _refreshTokenDal = refreshTokenDal;
        }
        public IDataResult<User> Login(UserLoginDto userLoginDto)
        {
            var userToCheck = _userService.GetByEmail(userLoginDto.Email);
            if (userToCheck == null)
            {
                return new ErrorDataResult<User>("User not found!");
            }

            if (!HashingHelper.VerifyPasswordHash(userLoginDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>("Sifre yanlış");
            }

            return new SuccessDataResult<User>(userToCheck);

        }

        public IDataResult<User> Register(UserRegisterDto registerDto, string password)
        {
            try
            {
                byte[] passwordHash, passwordSalt;
                HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
                var user = new User
                {

                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    Email = registerDto.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };
                _userService.Add(user);
                return new SuccessDataResult<User>(user);
                //var checkUser = new TurkishCitizenIdentity(12345679810, registerDto.FirstName, registerDto.LastName, 1900).IsValid();
                //if (checkUser)
                //{
                //    _userService.Add(user);
                //    return new SuccessDataResult<User>(user);
                //}
                //return new ErrorDataResult<User>("TC kimlik numarası kişi uyumu sağlanamadı!");

            }
            catch (Exception e)
            {
                return new ErrorDataResult<User>(e.Message);
            }
        }
        
        public IResult UserExists(string email)
        {
            if (_userService.GetByEmail(email) != null)
            {
                return new ErrorResult("User already exists");
            }
            return new SuccessResult();
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            var claims = _userService.GetClaim(user);
            var accessToken = _tokenHelper.CreateToken(user, claims);
            return new SuccessDataResult<AccessToken>(accessToken);
        }

        public User UserExistsId(int id)
        {
            var user = _userService.GetById(id);
            return user ?? null;
        }

        public IDataResult<AccessToken> RefreshToken(string token, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);

            if (validatedToken == null)
            {
                return new ErrorDataResult<AccessToken>("Invalid Token");
            }

            var expiryDateUnix =
                long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                return new ErrorDataResult<AccessToken>("This token hasn't expired yet");
                
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = _refreshTokenDal.Get(x => x.Token == refreshToken);

            if (storedRefreshToken == null)
            {
                return new ErrorDataResult<AccessToken>("This refresh token does not exist");
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return new ErrorDataResult<AccessToken>("This refresh token has expired");
            }

            if (storedRefreshToken.Invalidated)
            {
                return new ErrorDataResult<AccessToken>("This refresh token has been invalidated");
            }

            if (storedRefreshToken.Used)
            {
                return new ErrorDataResult<AccessToken>("This refresh token has been used");
            }

            if (storedRefreshToken.JwtId != jti)
            {
                return new ErrorDataResult<AccessToken>("This refresh token does not match this JWT");
            }

            storedRefreshToken.Used = true;
           _refreshTokenDal.Update(storedRefreshToken);

           var user = _userService.GetById(Convert.ToInt32(validatedToken.Claims.Single(x => x.Type == "Id").Value));
           return  new SuccessDataResult<AccessToken>(CreateAccessToken(user).Data);
        }
        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var _tokenValidationParameters = new TokenValidationParameters();
                var tokenValidationParameters = _tokenValidationParameters.Clone();
                tokenValidationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }
        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                       StringComparison.InvariantCultureIgnoreCase);
        }

        public IResult Delete(User user)
        {
            _userDal.Delete(user);
            return new SuccessResult("UserDeleted");
        }

        public IDataResult<User> UserUpdate(User userToCheck, UserUpdateDto userUpdateDto, string password = null)
        {
            if (userUpdateDto.Email != userToCheck.Email)
            {
                userToCheck.Email = userUpdateDto.Email;
            }
            else
            {
                return new ErrorDataResult<User>("UserAlreadyExists");
            }
            //if (userUpdateDto.Tc != userToCheck.TcNo)
            //{
            //    try
            //    {
            //        var checkUser = new TurkishCitizenIdentity(12345679810, userUpdateDto.FirstName, userUpdateDto.LastName, 1900).IsValid();
            //        if (checkUser)
            //        {
            //            userToCheck.TcNo = userUpdateDto.Tc;
            //        }
            //        else
            //        {
            //            return new ErrorDataResult<User>("TC kimlik numarası kişi uyumu sağlanamadı!");
            //        }

            //    }
            //    catch (Exception e)
            //    {
            //        return new ErrorDataResult<User>(e.Message);
            //    }
            //}
            //else
            //{
            //    return new ErrorDataResult<Kullanici>(Messages.UseTc);
            //}

            userToCheck.FirstName = userUpdateDto.FirstName;
            userToCheck.LastName = userUpdateDto.LastName;

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            userToCheck.PasswordHash = passwordHash;
            userToCheck.PasswordSalt = passwordSalt;


            _userDal.Update(userToCheck);
            return new SuccessDataResult<User>("UserUpdated");

        }
    }
}
