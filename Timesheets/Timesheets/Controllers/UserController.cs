using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Timesheets.Controllers.Requests;
using Timesheets.DataAccessLayer.Models;
using Timesheets.DataAccessLayer.Services.Interfaces;

namespace Timesheets.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private static ILogger<UserController> _logger;
        private IUserService _userService;
        private IRoleService _roleService;

        public UserController(
            ILogger<UserController> logger,
            IUserService userService,
            IRoleService roleService
            )
        {
            _logger = logger;
            _userService = userService;
            _roleService = roleService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult RegisterUser([FromQuery] string login, string password, int roleid)
        {
            _logger.LogInformation("RegisterUser() запуск метода");
            UserModel response = null;
            var message = "";
            var done = false;

            try
            {
                UserRequest userRequest = new UserRequest()
                {
                    Login = login,
                    Password = password,
                    RoleId = roleid
                };

                response = _userService.RegisterUser(userRequest);
                if (response != null)
                {
                    done = true;
                }
            }
            catch (Exception ex)
            {
                message = $"добавить пользователя не удалось, ошибка {ex.Message}";
            }

            if (done)
            {
                _logger.LogInformation("RegisterUser() завершено");
                return Ok(response);
            }
            else
            {
                _logger.LogError($"RegisterUser() ошибка, {message}");
                return Ok(message);
            }
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromQuery] string login, string password)
        {
            _logger.LogInformation("Authenticate() запуск метода");
            UserModel response = _userService.GetUser(new UserRequest()
            {
                Login = login,
                Password = password
            });

            if (response != null)
            {
                //пользователь найден, получим роль
                var role = _roleService.GetRole(response.RoleId);
                if (role != null) {
                    //роль найдена
                    string value = _userService.GenerateJwtToken(login, role.Name);
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        return Ok(value);
                    }
                }
                return Ok($"токен для пользователя login = {login} не сформирован, ошибка");
            }
            else
            {
                var message = $"пользователь id = {login} не найден";
                _logger.LogError($"Authenticate() ошибка, {message}");
                return Ok(message);
            }
        }

        //получить пользователя по userid
        [Authorize(Roles = "Administrator")]
        [HttpGet("{login}")]
        public IActionResult GetUser([FromRoute] string login)
        {
            _logger.LogInformation("GetUser() запуск метода");
            UserModel response = _userService.GetUser(new UserRequest()
            {
                Login = login
            });
            
            if (response != null)
            {
                //пользователь найден
                return Ok(response);
            }
            else
            {
                var message = $"пользователь login = {login} не найден";
                _logger.LogError($"GetUser() ошибка, {message}");
                return Ok(message);
            }
        }

        //получить всех пользователей
        [Authorize(Roles = "Administrator")]
        [HttpGet("all")]
        public IActionResult GetUserAll()
        {
            _logger.LogInformation("GetUserAll() запуск метода");
            List<UserModel> response = _userService.GetUserAll();
            _logger.LogInformation("GetUserAll() завершено");
            return Ok(response);
        }
    }
}
