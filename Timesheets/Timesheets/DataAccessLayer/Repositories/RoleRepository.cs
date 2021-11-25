using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Timesheets.Data;
using Timesheets.DataAccessLayer.Models;
using Timesheets.DataAccessLayer.Repositories.Interfaces;

namespace Timesheets.DataAccessLayer.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private TimesheetContext _context;
        private readonly ILogger<RoleRepository> _logger;

        public RoleRepository(
            TimesheetContext context,
            ILogger<RoleRepository> logger
            )
        {
            _context = context;
            _logger = logger;
        }

        public int RegisterRole(RoleDto role)
        {
            _logger.LogInformation("RegisterRole() запуск метода");
            if (role != null)
            {
                try
                {
                    _context.Roles.Add(role);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"RegisterRole() ошибка, {ex.Message}");
                }
                return role.Id;
            }
            return 0;
        }

        public RoleDto GetRole(int roleid)
        {
            _logger.LogInformation("GetRole() запуск метода");
            if (roleid > 0)
            {
                return _context.Roles
                    .Where(row => row.Id == roleid)
                    .SingleOrDefault();
            }
            return null;
        }    

        public List<RoleDto> GetRolesAll()
        {
            _logger.LogInformation("GetRolesAll() запуск метода");
            return _context.Roles
                .ToList();
        }      
    }
}
