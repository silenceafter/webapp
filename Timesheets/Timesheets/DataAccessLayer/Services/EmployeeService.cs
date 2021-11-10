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
    public class EmployeeService : IEmployeeService
    {
        private TimesheetContext _context;
        private readonly ILogger<EmployeeService> _logger;
        private ITaskEmployeeService _taskEmployeeService;
        private IEmployeeRepository _employeeRepository;

        public EmployeeService(
            TimesheetContext context,
            ILogger<EmployeeService> logger,
            ITaskEmployeeService taskEmployeeService,
            IEmployeeRepository employeeRepository
            )
        {
            _context = context;
            _logger = logger;
            _taskEmployeeService = taskEmployeeService;
            _employeeRepository = employeeRepository;
        }

        public EmployeeModel RegisterEmployee(EmployeeRequest employee)
        {
            _logger.LogInformation("RegisterEmployee() запуск метода");
            var newId = _employeeRepository.RegisterEmployee(new EmployeeDto()
            {
                BankAccount = employee.BankAccount,
                Rate = employee.Rate
            });

            if (newId > 0)
            {
                _logger.LogInformation("RegisterEmployee() завершено");
                return new EmployeeModel()
                {
                    Id = newId,
                    BankAccount = employee.BankAccount,
                    Rate = employee.Rate,
                    Tasks = new List<int>()
                };
            }
            else
            {
                return null;
            }
        }

        public EmployeeModel GetEmployee(int id)
        {
            _logger.LogInformation("GetEmployee() запуск метода");
            var employee = _employeeRepository.GetEmployee(id);
            if (employee != null)
            {
                //запрос к TaskEmployee
                List<int> tasks = new List<int>();
                var taskEmployeeModels = _taskEmployeeService.GetTaskEmployeeAll(employee);
                if (taskEmployeeModels != null)
                {
                    foreach (var taskEmployeeModel in taskEmployeeModels)
                    {
                        tasks.Add(taskEmployeeModel.TaskId);
                    }
                }

                _logger.LogInformation("GetEmployee() завершено");
                return new EmployeeModel()
                {
                    Id = employee.Id,
                    BankAccount = employee.BankAccount,
                    Rate = employee.Rate,
                    Tasks = tasks
                };
            }
            return null;
        }

        public bool SetEmployee(EmployeeModel employee)
        {
            _logger.LogInformation("SetEmployee() запуск метода");
            return _employeeRepository.SetEmployee(new EmployeeDto()
            {
                Id = employee.Id,
                BankAccount = employee.BankAccount,
                Rate = employee.Rate
            });
        }

        public List<EmployeeModel> GetEmployeeAll()
        {
            _logger.LogInformation("GetEmployeeAll() запуск метода");
            List<EmployeeModel> employees = new List<EmployeeModel>();
            var employeesDto = _employeeRepository.GetEmployeeAll();
            if (employeesDto != null)
            {                
                foreach(var employeeDto in employeesDto)
                {
                    //запрос к TaskEmployee
                    List<int> tasks = new List<int>();
                    var taskEmployeeModels = _taskEmployeeService.GetTaskEmployeeAll(employeeDto);
                    if (taskEmployeeModels != null)
                    {
                        foreach (var taskEmployeeModel in taskEmployeeModels)
                        {
                            tasks.Add(taskEmployeeModel.TaskId);
                        }
                    }

                    employees.Add(new EmployeeModel()
                    {
                        Id = employeeDto.Id,
                        BankAccount = employeeDto.BankAccount, 
                        Rate = employeeDto.Rate,
                        Tasks = tasks
                    });
                }
            }
            _logger.LogInformation("GetEmployeeAll() завершено");
            return employees;
        }        
    }
}