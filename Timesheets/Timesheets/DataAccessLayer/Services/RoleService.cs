using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Timesheets.Controllers.Requests;
using Timesheets.DataAccessLayer.Models;
using Timesheets.DataAccessLayer.Repositories.Interfaces;
using Timesheets.DataAccessLayer.Services.Interfaces;

namespace Timesheets.DataAccessLayer.Services
{
    public class RoleService : IRoleService
    {
        private readonly ILogger<RoleService> _logger;
        private IRoleRepository _roleRepository;

        public RoleService(
            ILogger<RoleService> logger,
            IRoleRepository roleRepository
            )
        {
            _logger = logger;
            _roleRepository = roleRepository;
        }

        public RoleModel RegisterRole(RoleRequest role)
        {
            _logger.LogInformation("RegisterRole() запуск метода");
            var newId = _roleRepository.RegisterRole(new RoleDto()
            {
                Name = role.Name
            });

            if (newId > 0)
            {
                _logger.LogInformation("RegisterRole() завершено");
                return new RoleModel()
                {
                    Id = newId,
                    Name = role.Name
                };
            }
            else
            {
                return null;
            }
        }

        public RoleModel GetRole(int roleid)
        {
            _logger.LogInformation("GetRole() запуск метода");
            if (roleid > 0)
            {
                var role = _roleRepository.GetRole(roleid);
                if (role != null)
                {
                    _logger.LogInformation("GetRole() завершено");
                    return new RoleModel()
                    {
                        Id = role.Id,
                        Name = role.Name
                    };
                }
            }            
            return null;
        }

        public List<RoleModel> GetRoleAll()
        {
            _logger.LogInformation("GetRoleAll() запуск метода");
            var rolesDto = _roleRepository.GetRolesAll();
            List<RoleModel> roles = new List<RoleModel>();
            if (rolesDto != null)
            {
                foreach (var roleDto in rolesDto)
                {
                    roles.Add(new RoleModel()
                    {
                        Id = roleDto.Id,
                        Name = roleDto.Name
                    });
                }
            }
            _logger.LogInformation("GetRoleAll() завершено");
            return roles;
        }  
    }
}
