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
    public class TaskService : ITaskService
    {
        private readonly DictionariesGlobal _dictionaryGlobal;
        private ITaskRepository _taskRepository;
        private IContractRepository _contractRepository;
        private ICustomerRepository _customRepository;

        public TaskService(
            DictionariesGlobal dictionariesGlobal, 
            ITaskRepository taskRepository,
            IContractRepository contractRepository,
            ICustomerRepository customerRepository
            )
        {
            _dictionaryGlobal = dictionariesGlobal;
            _taskRepository = taskRepository;
        }

        public TaskResponse RegisterTask(TaskModel task)
        {
            TaskResponse response = null;
            try
            {
                if (_taskRepository.RegisterTask(task))
                {
                    //сохранение прошло
                    response = new TaskResponse()
                    {
                        Id = task.Id,
                        Contract = task.Contract.Id,
                        Employees = new List<int>()
                    };
                }
            }
            catch (Exception ex)
            {
                //
            }
            return response;
        }

        public TaskResponse GetTask(int id, int contractid, int taskid)
        {
            TaskResponse response = null;
            TaskModel task = _taskRepository.GetTask(id, contractid, taskid);
            if (task != null)
            {
                //задача нашлась
                //для response оставим только id, не ссылки на объекты             
                List<int> responseEmployees = new List<int>();
                foreach (var employee in task.Employees)
                {
                    responseEmployees.Add(employee.Id);
                }

                response = new TaskResponse()
                {
                    Id = task.Id,
                    Contract = task.Contract.Id,
                    Employees = responseEmployees
                };
            }
            return response;
        }

        public List<TaskResponse> GetTaskAll(int id, int contractid)
        {
            List<TaskResponse> taskResponse = null;
            List<TaskModel> tasks = _taskRepository.GetTaskAll(id, contractid);

            if (tasks != null)
            {
                taskResponse = new List<TaskResponse>();
                foreach (var task in tasks)
                {
                    List<int> employeesResponse = new List<int>();
                    foreach (var employee in task.Employees)
                    {
                        employeesResponse.Add(employee.Id);
                    }

                    taskResponse.Add(new TaskResponse() 
                    {
                        Id = task.Id,
                        Contract = task.Contract.Id,
                        Employees = employeesResponse
                    });
                }
            }
            return taskResponse;
        }
        public TaskModel GetModel(int id, int contractid, int taskid)
        {
            TaskModel task = _taskRepository.GetTask(id, contractid, taskid);
            if (task != null)
            {
                //объект найден
                return task;
            }
            return null;
        }
    }
}
