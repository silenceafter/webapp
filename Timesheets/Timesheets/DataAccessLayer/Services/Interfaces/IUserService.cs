using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Timesheets.Controllers.Requests;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Services.Interfaces
{
    public interface IUserService
    {
        UserModel RegisterUser(UserRequest user);
        UserModel GetUser(UserRequest user);
        bool SetUser(UserModel user);
        List<UserModel> GetUserAll();
        string GenerateJwtToken(string login, string role);
    }
}
