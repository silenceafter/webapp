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
    public class UserRepository : IUserRepository
    {
        private TimesheetContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(
            TimesheetContext context,
            ILogger<UserRepository> logger
            )
        {
            _context = context;
            _logger = logger;
        }

        public int RegisterUser(UserDto user)
        {
            _logger.LogInformation("RegisterUser() запуск метода");
            if (user != null)
            {
                try
                {
                    _context.Users.Add(user);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"RegisterUser() ошибка, {ex.Message}");
                }
                return user.Id;
            }
            return 0;
        }

        public UserDto GetUser(UserDto user)
        {
            _logger.LogInformation("GetUser() запуск метода");
            if (user != null)
            {
                return _context.Users
                    .Where(row => row.Login == user.Login)
                    .SingleOrDefault();
            }
            return null;
            
        }

        public bool SetUser(UserDto user)
        {
            _logger.LogInformation("SetUser() запуск метода");
            if (user != null)
            {
                try
                {
                    var response = this.GetUser(user);
                    if (response != null)
                    {
                        response.Login = user.Login;
                        response.Password = user.Password;
                        response.RoleId = user.RoleId;
                    }
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"SetUser() ошибка, {ex.Message}");
                }
                return true;
            }
            return false;
        }

        public List<UserDto> GetUsersAll()
        {
            _logger.LogInformation("GetUsersAll() запуск метода");
            return _context.Users
                .ToList();
        }
    }
}
