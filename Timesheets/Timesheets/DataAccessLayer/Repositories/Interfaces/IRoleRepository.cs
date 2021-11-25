using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        int RegisterRole(RoleDto role);
        RoleDto GetRole(int roleid);
        List<RoleDto> GetRolesAll();
    }
}
