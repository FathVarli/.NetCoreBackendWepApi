using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using Business.Abstract;
using Core.CrossCuttingConcerns.Logging.Log4Net;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using Entity.Dtos;
using Entity.Dtos.UserDto;
using Microsoft.AspNetCore.Http;

namespace MentalBit.KutuphaneSistemi.Business.Concrete
{
    public class ForgotPasswordManager : IForgotPasswordService
    {
        private IPasswordResetDal _sifreYenilemeDal;
        private IUserDal _userDal;
        private IUserService _userService;
        private IMailService _mailService;
        private ILoggerService _loggerService;


        public ForgotPasswordManager(IPasswordResetDal sifreYenilemeDal,IMailService mailService, ILoggerService loggerService, IUserDal userDal, IUserService userService)
        {
            _sifreYenilemeDal = sifreYenilemeDal;
            _mailService = mailService;
            _loggerService = loggerService;
            _userDal = userDal;
            _userService = userService;
        }


        private IResult CreatePasswordResetToken(string Email)
        {
            var user = _userService.GetByEmail(Email);
            if (user != null)
            {
                var token = Guid.NewGuid().ToString();
                var expirationTime = DateTime.Now.AddDays(1);
                var resetUser = new ResetPassword
                {
                    UserId = user.Id,
                    Token = token,
                    ExpirationTime = expirationTime

                };
                _sifreYenilemeDal.Add(resetUser);
                return new SuccessResult();
            }
            _loggerService.Info("CreatePasswordResetToken: " + "UserNotFound");
            return new ErrorResult("UserNotFound");
        }

        private string CreatePasswordResetLink(int userId)
        {
            var context = new HttpContextAccessor();
            var user = _sifreYenilemeDal.Get(u => u.UserId == userId);
            var contextPath =context.HttpContext.Request.Host;  //_context.HttpContext.Request.Path;
            var link ="http://"+contextPath+"/api/auth/changepassword?id="+user.UserId+"&token="+user.Token;
            return link;
        }
        public IResult PasswordResetMail(ForgotPasswordDto forgotPasswordDto)
        {
            var result = CreatePasswordResetToken(forgotPasswordDto.Email);
            if (result.Success)
            {
                var user = _userService.GetByEmail(forgotPasswordDto.Email);
                var link = CreatePasswordResetLink(user.Id);
                var sendModel = new EmailSenderModel
                {
                    To = forgotPasswordDto.Email,
                    Subject = "Password Reset",
                    Body = "Click to link for reset password \n"+link,

                };

                _mailService.SendMail(sendModel);
                return new SuccessResult("Send password reset mail!");
            }
            return new ErrorResult(result.Message);
        }

        public IResult ValidatePasswordResetToken(int userId,string token)
        {
            var checkUser = _sifreYenilemeDal.Get(u => u.Id == userId && u.Token == token);
            if (checkUser!=null)
            {
                var sinir = checkUser.ExpirationTime - DateTime.Now;
                var dakikaSinir = sinir.Hours * 60 + sinir.Minutes + sinir.Seconds * (1 / 60);
                if (dakikaSinir>0)
                {
                    return new SuccessResult();
                }
            }
            _loggerService.Info("ValidatePassordResetToken error!");
            return new ErrorResult();
        }

        public IResult CreateNewPassword(PasswordResetDto passwordResetDto,int id)
        {
            var deletedToken = _sifreYenilemeDal.Get(u => u.UserId == id);
            if (deletedToken!=null)
            {
                byte[] passwordHash, passwordSalt;
                HashingHelper.CreatePasswordHash(passwordResetDto.Password, out passwordHash, out passwordSalt);

                var userExists = _userService.GetById(id);

                if (userExists == null)
                {
                    return new ErrorResult("UserNotFound");
                }
                userExists.PasswordHash = passwordHash;
                userExists.PasswordSalt = passwordSalt;
                _userDal.Update(userExists);
                _sifreYenilemeDal.Delete(deletedToken);
                return new SuccessResult("Success password reset!");
            }
            return new ErrorResult("UserNotFound");
            
        }
    }
}
