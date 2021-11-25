using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Timesheets.Data;
using Timesheets.DataAccessLayer.Models;
using Timesheets.DataAccessLayer.Repositories.Interfaces;
using Timesheets.DataAccessLayer.Services.Interfaces;

namespace Timesheets.DataAccessLayer.Repositories
{
    public class TaskEmployeeRepository : ITaskEmployeeRepository
    {
        private TimesheetContext _context;
        private readonly ILogger<TaskEmployeeRepository> _logger;

        public TaskEmployeeRepository(
            TimesheetContext context,
            ILogger<TaskEmployeeRepository> logger
            )
        {
            _context = context;
            _logger = logger;
        }

        public int RegisterTaskEmployee(TaskEmployeeDto taskEmployee)
        {
            _logger.LogInformation("RegisterTaskEmployee() запуск метода");
            if (taskEmployee != null)
            {
                bool done = false;
                TaskEmployeeDto taskEmployeeDto = new TaskEmployeeDto()
                {
                    Hours = taskEmployee.Hours,
                    Done = taskEmployee.Done,
                    EmployeeId = taskEmployee.EmployeeId,
                    TaskId = taskEmployee.TaskId
                };

                try
                {
                    _context.TaskEmployees.Add(taskEmployeeDto);
                    _context.SaveChanges();
                    done = true;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"RegisterTaskEmployee() ошибка, {ex.Message}");
                }

                if (done)
                {
                    _logger.LogInformation("RegisterTaskEmployee() завершено");
                    return taskEmployeeDto.Id;
                }
            }
            return 0;
        }
        
        public TaskEmployeeDto GetTaskEmployee(int taskid, int employeeid)
        {
            //запись из бд
            _logger.LogInformation("GetTaskEmployee() запуск метода");
            return _context.TaskEmployees
                .Where(row => row.TaskId == taskid)
                .Where(row => row.EmployeeId == employeeid).SingleOrDefault();
        }

        public TaskEmployeeDto GetTaskEmployee(int taskEmployeeId)
        {
            //запись из бд
            _logger.LogInformation("GetTaskEmployee() запуск метода");
            return _context.TaskEmployees.Where(row => row.Id == taskEmployeeId).SingleOrDefault();
        }

        public bool SetTaskEmployee(TaskEmployeeDto taskEmployee)
        {
            _logger.LogInformation("SetTaskEmployee() запуск метода");
            if (taskEmployee != null)
            {
                try
                {
                    var response = this.GetTaskEmployee(taskEmployee.Id);
                    if (response != null)
                    {
                        response.Done = taskEmployee.Done;
                        response.Hours = taskEmployee.Hours;
                    }
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"SetTaskEmployee() ошибка, {ex.Message}");
                }
                return true;
            }
            _logger.LogInformation("SetTaskEmployee() завершено");
            return false;
        }

        public List<TaskEmployeeDto> GetTaskEmployeeAll(int taskid)
        {
            _logger.LogInformation("GetTaskEmployeeAll() запуск метода");
            return _context.TaskEmployees
                .Where(row => row.TaskId == taskid).ToList();
        }

        public List<TaskEmployeeDto> GetTaskEmployeeAll(EmployeeDto employee)
        {
            _logger.LogInformation("GetTaskEmployeeAll() запуск метода");
            return _context.TaskEmployees
                .Where(row => row.EmployeeId == employee.Id).ToList();
        }
    }
}
