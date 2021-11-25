using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Requests;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Services.Interfaces
{
    public interface IRoleService
    {
        RoleModel RegisterRole(RoleRequest role);
        RoleModel GetRole(int roleid);
        List<RoleModel> GetRoleAll();
    }
}
