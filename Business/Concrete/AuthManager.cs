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
using Microsoft.IdentityModel.Tokens;
using TurkishCitizenIdValidator;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;
        private IUserDal _userDal;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper, IUserDal userDal)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _userDal = userDal;
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
