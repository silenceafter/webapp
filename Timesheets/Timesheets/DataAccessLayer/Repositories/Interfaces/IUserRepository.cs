using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Repositories.Interfaces
{
    public interface IUserRepository
    {
        int RegisterUser(UserDto user);
        UserDto GetUser(UserDto user);
        bool SetUser(UserDto user);
        List<UserDto> GetUsersAll();
    }
}
