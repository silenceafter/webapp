using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Responses;
using Timesheets.DataAccessLayer.Interfaces;
using Timesheets.DataAccessLayer.Models;
using Timesheets.DataAccessLayer.Repositories.Interfaces;

namespace Timesheets.DataAccessLayer.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DictionariesGlobal _dictionaryGlobal;
        private IContractRepository _contractRepository;

        public EmployeeRepository(DictionariesGlobal dictionariesGlobal, IContractRepository contractRepository)
        {
            _dictionaryGlobal = dictionariesGlobal;
            _contractRepository = contractRepository;
        }

        public bool RegisterEmployee(EmployeeModel employee)
        {
            _dictionaryGlobal.employees.Add(employee); //сохраняем работника            
            _dictionaryGlobal._employeeid++;
            return true;
        }

        public EmployeeModel GetEmployee(int id)
        {
            var employees = _dictionaryGlobal.employees;
            EmployeeModel currentEmployee = null;
            foreach (var employee in employees)
            {
                if (employee.Id.Equals(id))
                {
                    //работник найден
                    currentEmployee = employee;
                    break;
                }
            }
            return currentEmployee;
        }

        public List<EmployeeModel> GetEmployeeAll()
        {
            var employees = _dictionaryGlobal.employees;
            return employees;
        }

        public bool RegisterTaskToEmployee(EmployeeModel employee, TaskModel task, TaskEmployeeModel taskEmployee)
        {
            var done = false;
            try
            {
                task.Employees.Add(taskEmployee);
                employee.Tasks.Add(task);
                _dictionaryGlobal._taskEmployeeId++;
                done = true;
            }
            catch
            {
            }
            return done;
        }

        public TaskModel GetTask(int id, int taskid)
        {
            TaskModel currentTask = null;
            EmployeeModel employee = this.GetEmployee(id);
            if(employee != null)
            {
                //работник найден
                var tasks = employee.Tasks;
                foreach(var task in tasks)
                {
                    if (task.Id.Equals(taskid))
                    {
                        //задача найдена
                        currentTask = task;
                        break;
                    }
                }
            }
            return currentTask;    
        }

        public List<TaskModel> GetTaskAll(int id)
        {
            List<TaskModel> tasks = null;
            EmployeeModel employee = this.GetEmployee(id);
            if (employee != null)
            {
                //работник найден
                tasks = employee.Tasks;                
            }
            return tasks;
        }
    }
}