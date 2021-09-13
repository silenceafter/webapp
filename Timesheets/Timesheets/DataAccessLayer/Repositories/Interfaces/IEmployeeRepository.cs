using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.DataAccessLayer.Interfaces;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Repositories.Interfaces
{
    public interface IEmployeeRepository : IRepository<EmployeeModel>
    {
        bool RegisterEmployee(EmployeeModel employee);
        EmployeeModel GetEmployee(int id);
        List<EmployeeModel> GetEmployeeAll();
        bool RegisterTaskToEmployee(EmployeeModel employee, TaskModel task, TaskEmployeeModel taskEmployee);
        TaskModel GetTask(int id, int taskid);
        List<TaskModel> GetTaskAll(int id);
    }
}
