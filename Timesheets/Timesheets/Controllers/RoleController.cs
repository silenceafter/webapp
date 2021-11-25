using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Timesheets.Controllers.Requests;
using Timesheets.DataAccessLayer.Models;
using Timesheets.DataAccessLayer.Services.Interfaces;

namespace Timesheets.Controllers
{
    [Route("api/role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private static ILogger<RoleController> _logger;
        private IRoleService _roleService;

        public RoleController(
            ILogger<RoleController> logger,
            IRoleService roleService
            )
        {
            _logger = logger;
            _roleService = roleService;
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("register")]
        public IActionResult RegisterRole([FromQuery] string name)
        {
            _logger.LogInformation("RegisterRole() запуск метода");
            RoleModel response = null;
            var message = "";
            var done = false;

            try
            {
                RoleRequest roleRequest = new RoleRequest()
                {
                    Name = name
                };

                response = _roleService.RegisterRole(roleRequest);
                if (response != null)
                {
                    done = true;
                }
            }
            catch (Exception ex)
            {
                message = $"добавить роль не удалось, ошибка {ex.Message}";
            }

            if (done)
            {
                _logger.LogInformation("RegisterRole() завершено");
                return Ok(response);
            }
            else
            {
                _logger.LogError($"RegisterRole() ошибка, {message}");
                return Ok(message);
            }
        }

        //получить все роли
        [Authorize(Roles = "Administrator")]
        [HttpGet("all")]
        public IActionResult GetRoleAll()
        {
            _logger.LogInformation("GetRoleAll() запуск метода");
            List<RoleModel> response = _roleService.GetRoleAll();
            _logger.LogInformation("GetRoleAll() завершено");
            return Ok(response);
        }
    }
}
