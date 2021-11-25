using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Timesheets.Controllers.Requests;
using Timesheets.Data;
using Timesheets.DataAccessLayer.Models;
using Timesheets.DataAccessLayer.Repositories.Interfaces;
using Timesheets.DataAccessLayer.Services.Interfaces;

namespace Timesheets.DataAccessLayer.Services
{
    public class TaskEmployeeService : ITaskEmployeeService
    {
        private TimesheetContext _context;
        private readonly ILogger<TaskEmployeeService> _logger;
        private ITaskEmployeeRepository _taskEmployeeRepository;        

        public TaskEmployeeService(
            TimesheetContext context,
            ILogger<TaskEmployeeService> logger,
            ITaskEmployeeRepository taskEmployeeRepository
            )
        {
            _context = context;
            _logger = logger;
            _taskEmployeeRepository = taskEmployeeRepository;
        }

        public TaskEmployeeModel RegisterTaskEmployee(TaskEmployeeRequest taskEmployee)
        {
            _logger.LogInformation("RegisterTaskEmployee() запуск метода");
            var newId = _taskEmployeeRepository.RegisterTaskEmployee(new TaskEmployeeDto()
            {
                Hours = 0.0,
                Done = false,
                EmployeeId = taskEmployee.Employee,
                TaskId = taskEmployee.Task
            });

            if (newId > 0)
            {
                _logger.LogInformation("RegisterTaskEmployee() завершено");
                return new TaskEmployeeModel()
                {
                    Id = newId,
                    Hours = 0.0,
                    Done = false,
                    EmployeeId = taskEmployee.Employee,
                    TaskId = taskEmployee.Task
                };
            }
            else
            {
                return null;
            }
        }        

        public TaskEmployeeModel GetTaskEmployee(int taskid, int employeeid)
        {
            _logger.LogInformation("GetTaskEmployee() запуск метода");
            var taskEmployee = _taskEmployeeRepository.GetTaskEmployee(taskid, employeeid);
            if (taskEmployee != null)
            {
                _logger.LogInformation("GetTaskEmployee() завершено");
                return new TaskEmployeeModel()
                {
                    Id = taskEmployee.Id, 
                    Hours = taskEmployee.Hours,
                    Done = taskEmployee.Done,
                    EmployeeId = taskEmployee.EmployeeId,
                    TaskId = taskEmployee.TaskId
                };
            }
            return null;
        }

        public bool SetTaskEmployee(TaskEmployeeModel taskEmployee)
        {
            _logger.LogInformation("SetTaskEmployee() запуск метода");
            if (taskEmployee != null)
            {
                _logger.LogInformation("SetTaskEmployee() завершено");
                return _taskEmployeeRepository.SetTaskEmployee(new TaskEmployeeDto()
                {
                    Id = taskEmployee.Id,
                    EmployeeId = taskEmployee.EmployeeId,
                    TaskId = taskEmployee.TaskId,
                    Done = taskEmployee.Done,
                    Hours = taskEmployee.Hours
                });
            }
            return false;
        }

        public List<TaskEmployeeModel> GetTaskEmployeeAll(int taskid)
        {
            _logger.LogInformation("GetTaskEmployeeAll() запуск метода");
            List<TaskEmployeeModel> taskEmployees = new List<TaskEmployeeModel>();
            var taskEmployeesDto = _taskEmployeeRepository.GetTaskEmployeeAll(taskid);
            if (taskEmployeesDto != null)
            { 
                foreach(var taskEmployeeDto in taskEmployeesDto)
                {
                    taskEmployees.Add(new TaskEmployeeModel()
                    {
                        Id = taskEmployeeDto.Id,
                        Hours = taskEmployeeDto.Hours,
                        Done = taskEmployeeDto.Done,
                        EmployeeId = taskEmployeeDto.EmployeeId,
                        TaskId = taskEmployeeDto.TaskId
                    });
                }
            }
            _logger.LogInformation("GetTaskEmployeeAll() завершено");
            return taskEmployees;
        }

        public List<TaskEmployeeModel> GetTaskEmployeeAll(EmployeeDto employee)
        {
            _logger.LogInformation("GetTaskEmployeeAll() запуск метода");
            List<TaskEmployeeModel> taskEmployees = new List<TaskEmployeeModel>();
            var taskEmployeesDto = _taskEmployeeRepository.GetTaskEmployeeAll(employee);
            if (taskEmployeesDto != null)
            {
                foreach (var taskEmployeeDto in taskEmployeesDto)
                {
                    taskEmployees.Add(new TaskEmployeeModel()
                    {
                        Id = taskEmployeeDto.Id,
                        Hours = taskEmployeeDto.Hours,
                        Done = taskEmployeeDto.Done,
                        EmployeeId = taskEmployeeDto.EmployeeId,
                        TaskId = taskEmployeeDto.TaskId
                    });
                }
            }
            _logger.LogInformation("GetTaskEmployeeAll() завершено");
            return taskEmployees;
        }   
    }
}
