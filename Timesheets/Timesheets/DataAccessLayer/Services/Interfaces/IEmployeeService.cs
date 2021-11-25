using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Requests;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Services.Interfaces
{
    public interface IEmployeeService
    {
        EmployeeModel RegisterEmployee(EmployeeRequest employee);
        EmployeeModel GetEmployee(int id);
        bool SetEmployee(EmployeeModel employee);
        List<EmployeeModel> GetEmployeeAll();
    }
}
