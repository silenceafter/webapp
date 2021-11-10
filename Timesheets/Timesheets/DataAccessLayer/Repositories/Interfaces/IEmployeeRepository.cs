using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.DataAccessLayer.Interfaces;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        int RegisterEmployee(EmployeeDto employee);
        EmployeeDto GetEmployee(int id);
        bool SetEmployee(EmployeeDto employee);
        List<EmployeeDto> GetEmployeeAll();
    }
}
