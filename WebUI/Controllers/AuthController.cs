using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Entities.Concrete;
using Entity.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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

    }
}