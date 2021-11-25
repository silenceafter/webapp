using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Requests;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Services.Interfaces
{
    public interface ITaskEmployeeService
    {
        TaskEmployeeModel RegisterTaskEmployee(TaskEmployeeRequest task);
        TaskEmployeeModel GetTaskEmployee(int taskid, int employeeid);
        bool SetTaskEmployee(TaskEmployeeModel taskEmployee);
        List<TaskEmployeeModel> GetTaskEmployeeAll(int taskid);
        List<TaskEmployeeModel> GetTaskEmployeeAll(EmployeeDto employee);
    }
}
