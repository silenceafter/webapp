using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.DataAccessLayer.Interfaces;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Repositories.Interfaces
{
    public interface ITaskEmployeeRepository
    {
        int RegisterTaskEmployee(TaskEmployeeDto task);
        TaskEmployeeDto GetTaskEmployee(int taskid, int employeeid);
        TaskEmployeeDto GetTaskEmployee(int taskEmployeeId);
        bool SetTaskEmployee(TaskEmployeeDto taskEmployee);
        List<TaskEmployeeDto> GetTaskEmployeeAll(int taskid);
        List<TaskEmployeeDto> GetTaskEmployeeAll(EmployeeDto employee);
    }
}
