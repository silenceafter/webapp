using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Timesheets.Controllers.Requests;
using Timesheets.Data;
using Timesheets.DataAccessLayer.Interfaces;
using Timesheets.DataAccessLayer.Models;
using Timesheets.DataAccessLayer.Repositories.Interfaces;
using Timesheets.DataAccessLayer.Services.Interfaces;

namespace Timesheets.DataAccessLayer.Services
{
    public class TaskService : ITaskService
    {
        private TimesheetContext _context;
        private readonly ILogger<TaskService> _logger;
        private ITaskRepository _taskRepository;     
        private ITaskEmployeeService _taskEmployeeService;

        public TaskService( 
            TimesheetContext context,
            ILogger<TaskService> logger,
            ITaskRepository taskRepository,            
            ITaskEmployeeService taskEmployeeService
            )
        {
            _context = context;
            _logger = logger;
            _taskRepository = taskRepository;            
            _taskEmployeeService = taskEmployeeService;
        }

        public TaskModel RegisterTask(TaskRequest task)
        {
            _logger.LogInformation("RegisterTask() запуск метода");
            var newId = _taskRepository.RegisterTask(new TaskDto()
            {
                ContractId = task.Contract,
                TaskEmployeeId = 0
            });

            if (newId > 0)
            {
                _logger.LogInformation("RegisterTask() завершено");
                return new TaskModel()
                {
                    Id = newId,
                    ContractId = task.Contract,
                    TaskEmployees = new List<int>()
                };
            }
            else
            {
                return null;
            }
        }

        public TaskModel GetTask(int customerid, int contractid, int taskid)
        {
            _logger.LogInformation("GetTask() запуск метода");
            var task = _taskRepository.GetTask(customerid, contractid, taskid);
            if (task != null)
            {
                //taskEmployees
                var taskEmployeesModels = _taskEmployeeService.GetTaskEmployeeAll(taskid);
                List<int> taskEmployees = new List<int>();
                if (taskEmployeesModels != null)
                {
                    foreach (var taskEmployeeModel in taskEmployeesModels)
                    {
                        taskEmployees.Add(taskEmployeeModel.Id);
                    };
                }

                _logger.LogInformation("GetTask() завершено");
                return new TaskModel()
                {
                    Id = task.Id,
                    ContractId = task.ContractId,
                    TaskEmployees = taskEmployees
                };
            }
            return null;
        }

        public TaskModel GetTask(int taskid)
        {
            _logger.LogInformation("GetTask() запуск метода");
            var task = _taskRepository.GetTask(taskid);
            if (task != null)
            {
                //taskEmployees
                var taskEmployeesModels = _taskEmployeeService.GetTaskEmployeeAll(taskid);
                List<int> taskEmployees = new List<int>();
                if (taskEmployeesModels != null)
                {
                    foreach (var taskEmployeeModel in taskEmployeesModels)
                    {
                        taskEmployees.Add(taskEmployeeModel.Id);
                    };
                }

                _logger.LogInformation("GetTask() завершено");
                return new TaskModel()
                {
                    Id = task.Id,
                    ContractId = task.ContractId,
                    TaskEmployees = taskEmployees
                };
            }
            return null;
        }

        public List<TaskModel> GetTaskAll(int contractid)
        {
            _logger.LogInformation("GetTaskAll() запуск метода");
            List<TaskModel> tasks = new List<TaskModel>();
            var tasksDto = _taskRepository.GetTaskAll(contractid);
            if (tasksDto != null)
            {
                foreach(var taskDto in tasksDto)
                {
                    //taskEmployees
                    var taskEmployeesModels = _taskEmployeeService.GetTaskEmployeeAll(taskDto.Id);
                    List<int> taskEmployees = new List<int>();
                    if (taskEmployeesModels != null)
                    {
                        foreach (var taskEmployeeModel in taskEmployeesModels)
                        {
                            taskEmployees.Add(taskEmployeeModel.Id);
                        };
                    }

                    tasks.Add(new TaskModel()
                    {
                        Id = taskDto.Id,
                        ContractId = taskDto.ContractId,
                        TaskEmployees = taskEmployees
                    });
                }
            }
            _logger.LogInformation("GetTaskAll() завершено");
            return tasks;
        }
    }
}
