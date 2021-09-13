using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Responses;
using Timesheets.DataAccessLayer.Interfaces;
using Timesheets.DataAccessLayer.Models;
using Timesheets.DataAccessLayer.Repositories.Interfaces;
using Timesheets.DataAccessLayer.Services.Interfaces;

namespace Timesheets.DataAccessLayer.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly DictionariesGlobal _dictionaryGlobal;
        private IEmployeeRepository _employeeRepository;

        public EmployeeService(DictionariesGlobal dictionariesGlobal, IEmployeeRepository employeeRepository)
        {
            _dictionaryGlobal = dictionariesGlobal;
            _employeeRepository = employeeRepository;
        }

        public EmployeeResponse RegisterEmployee(EmployeeModel employee)
        {
            EmployeeResponse response = null;
            try
            {
                if (_employeeRepository.RegisterEmployee(employee))
                {
                    //сохранение прошло
                    response = new EmployeeResponse()
                    {
                        Id = employee.Id,
                        BankAccount = employee.BankAccount,
                        Rate = employee.Rate,
                        Tasks = new List<int>()
                    };
                }
            }
            catch (Exception ex)
            {
                //
            }
            return response;
        }

        public EmployeeResponse GetEmployee(int id)
        {
            EmployeeResponse response = null;
            EmployeeModel employee = _employeeRepository.GetEmployee(id);
            if (employee != null)
            {
                //работник нашелся
                //для response оставим только id, не ссылки на объекты
                List<int> responseTasks = new List<int>();
                foreach (var task in employee.Tasks)
                {
                    responseTasks.Add(task.Id);
                }

                response = new EmployeeResponse()
                {
                    Id = employee.Id,
                    BankAccount = employee.BankAccount,
                    Rate = employee.Rate,
                    Tasks = responseTasks
                };
            }
            return response;
        }

        public List<EmployeeResponse> GetEmployeeAll()
        {
            List<EmployeeResponse> response = new List<EmployeeResponse>();
            List<EmployeeModel> employees = _employeeRepository.GetEmployeeAll();
            if(employees != null)
            {
                foreach (var employee in employees)
                {
                    List<int> tasks = new List<int>();
                    foreach(var task in employee.Tasks)
                    {
                        tasks.Add(task.Id);
                    }

                    response.Add(new EmployeeResponse()
                    {
                        Id = employee.Id,
                        BankAccount = employee.BankAccount, 
                        Rate = employee.Rate,
                        Tasks = tasks
                    });
                }
            }
            return response;
        }

        public TaskEmployeeResponse RegisterTaskToEmployee(EmployeeModel employee, TaskModel task, TaskEmployeeModel taskEmployee)
        {
            TaskEmployeeResponse response = null;
            try
            {
                if (_employeeRepository.RegisterTaskToEmployee(employee, task, taskEmployee))
                {
                    //сохранение прошло
                    response = new TaskEmployeeResponse()
                    {
                        Id = taskEmployee.Id,
                        Done = taskEmployee.Done,
                        Hours = taskEmployee.Hours,
                        Task = taskEmployee.Task.Id,
                        Employee = taskEmployee.Employee.Id
                    };
                }
            }
            catch
            {
            }
            return response;
        }

        public List<TaskResponse> GetTaskAll(EmployeeModel employee)
        {
            List<TaskModel> tasks = _employeeRepository.GetTaskAll(employee.Id);
            List<TaskResponse> response = new List<TaskResponse>();
            foreach(var task in tasks)
            {
                List<int> employeesResponse = new List<int>();
                foreach(var employeeResponse in task.Employees)
                {
                    employeesResponse.Add(employeeResponse.Id);
                }

                response.Add(new TaskResponse()
                {
                    Id = task.Id,
                    Contract = task.Contract.Id,
                    Employees = employeesResponse
                }
                );
            }
            return response;
        }
        public TaskEmployeeResponse GetTaskEmployee(int id, int taskid)
        {
            TaskEmployeeResponse response = null;
            TaskModel task = _employeeRepository.GetTask(id, taskid);
            if(task != null)
            {
                foreach(var taskEmployee in task.Employees)
                {
                    if (taskEmployee.Employee.Id.Equals(id))
                    {
                        //сущность найдена
                        response = new TaskEmployeeResponse()
                        {
                            Id = taskEmployee.Id,
                            Done = taskEmployee.Done,
                            Employee = taskEmployee.Employee.Id,
                            Hours = taskEmployee.Hours,
                            Task = taskEmployee.Task.Id
                        };
                    }
                }
            }       
            return response;
        }

        public EmployeeModel GetModel(EmployeeResponse employeeResponse)
        {
            if (employeeResponse != null)
            {
                EmployeeModel employee = _employeeRepository.GetEmployee(employeeResponse.Id);
                if (employee != null)
                {
                    //объект найден
                    return employee;
                }
            }
            return null;
        }        
    }
}