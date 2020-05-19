using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.IoC;
using Entity.Dtos;
using Entity.Dtos.UserDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using WebUI.RedisCache;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        private IForgotPasswordService _passwordService;
        private IUserService _userService;

        public AuthController(IAuthService authService, IForgotPasswordService passwordService, IUserService userService)
        {
            _authService = authService;
            _passwordService = passwordService;
            _userService = userService;
        }

        [HttpPost("login")]
        public ActionResult Login(UserLoginDto userLoginDto)
        {
            try
            {
                var userToLogin = _authService.Login(userLoginDto);
                if (!userToLogin.Success)
                {
                    return BadRequest(userToLogin.Message);
                }

                var result = _authService.CreateAccessToken(userToLogin.Data);
                if (result.Success)
                {
                    return Ok(result.Data);
                }

                return BadRequest(result.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
       
        [HttpPost("register")]
        public ActionResult Register(UserRegisterDto registerModel)
        {
            try
            {
                var userExists = _authService.UserExists(registerModel.Email);
                if (!userExists.Success)
                {
                    return BadRequest(userExists.Message);
                }

                var registerResult = _authService.Register(registerModel, registerModel.Password);
                if (registerResult.Success)
                {
                    var result = _authService.CreateAccessToken(registerResult.Data);
                    return Ok(result.Data);
                }
                return BadRequest(registerResult.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("{id}")]
        public ActionResult UserUpdate(int id, UserUpdateDto updateModel)
        {
            try
            {
                var userExists = _authService.UserExistsId(id);

                if (userExists == null)
                {
                    return BadRequest("UserNotFound");
                }

                var updateResult = _authService.UserUpdate(userExists, updateModel, updateModel.Password);
                if (updateResult.Success)
                {
                    return Ok(updateResult.Message);
                }

                return BadRequest(updateResult.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("delete")]
        public IActionResult Delete(User user)
        {
            var result = _authService.Delete(user);
            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("resetpassword")]
        public ActionResult PasswordReset(ForgotPasswordDto model)
        {
            var result = _passwordService.PasswordResetMail(model);
            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

        [HttpGet("changepassword")]
        public ActionResult ValidatePassword(int id, string token)
        {
            var result = _passwordService.ValidatePasswordResetToken(id, token);
            string clientUrl = "";
            var contextPath = HttpContext.Request.Host;
            bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            if (isDevelopment)
            {
                clientUrl = Environment.GetEnvironmentVariable("DEVOLOPMENT_CLIENT_URL");
            }
            else
            {
                //address of the real environment
            }

            if (result.Success)
            {
                string redirectUrl = string.Format(clientUrl + "/authentication/reset-password/{0}", id);
                return Redirect(redirectUrl);
            }
            return Redirect(clientUrl + "/authentication/login");
        }
        [HttpPost("changepasswordpage")]
        public ActionResult ChangePassword(PasswordResetDto model)
        {
            var result = _passwordService.CreateNewPassword(model, model.Id);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        [HttpGet("getalluser")]
        public ActionResult GetAllUser()
        {
            var result = _userService.GetUsers();

            return Ok(result);
        }

    }
}