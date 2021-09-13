using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Responses;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Services.Interfaces
{
    public interface IEmployeeService
    {
        EmployeeResponse RegisterEmployee(EmployeeModel employee);
        EmployeeResponse GetEmployee(int id);
        List<EmployeeResponse> GetEmployeeAll();
        TaskEmployeeResponse RegisterTaskToEmployee(EmployeeModel employee, TaskModel task, TaskEmployeeModel taskEmployee);
        List<TaskResponse> GetTaskAll(EmployeeModel employee);
        TaskEmployeeResponse GetTaskEmployee(int id, int taskid);
        EmployeeModel GetModel(EmployeeResponse employeeResponse);
    }
}
