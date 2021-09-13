using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Responses;

namespace Timesheets.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly DictionariesGlobal _dictionaryGlobal;
        public EmployeeController(DictionariesGlobal dictionariesGlobal)
        {
            _dictionaryGlobal = dictionariesGlobal;
        }

        //зарегистрировать работника
        [HttpPost("rate/{rate}")]
        public IActionResult RegisterEmployee([FromRoute] double rate)
        {
            var employees = _dictionaryGlobal.employees;
            var employeeId = _dictionaryGlobal._employeeid;
            EmployeeResponse response = null;
            var message = "";
            var done = false;

            try
            {
                EmployeeDto employee = new EmployeeDto()
                {
                    Id = employeeId,
                    BankAccount = 0,
                    Rate = rate,
                    Tasks = new List<TaskDto>()
                };
                employees.Add(employee);
                response = new EmployeeResponse()
                {
                    Id = employee.Id,
                    BankAccount = employee.BankAccount,
                    Rate = employee.Rate,
                    Tasks = new List<int>()
                };
                done = true;
            }
            catch (Exception ex)
            {
                message = $"добавить работника id = {employeeId} не удалось, ошибка {ex.Message}";
            }

            if (done)
            {
                _dictionaryGlobal._employeeid++;
                return Ok(response);
            }
            else
            {
                return Ok(message);
            }           
        }

        //получить работника по id
        [HttpGet("{id}")]
        public IActionResult GetEmployee([FromRoute] int id)
        {
            var employees = _dictionaryGlobal.employees;
            EmployeeResponse response = null;
            var find = false;

            foreach(var employee in employees)
            {
                if (employee.Id.Equals(id))
                {
                    //нашли работника
                    find = true;
                    List<int> tasks = new List<int>();
                    foreach(var task in employee.Tasks)
                    {
                        tasks.Add(task.Id);
                    }

                    response = new EmployeeResponse()
                    {
                        Id = employee.Id,
                        BankAccount = employee.BankAccount,
                        Rate = employee.Rate,
                        Tasks = tasks
                    };
                    break;                    
                }
            }

            if (find)
            {
                return Ok(response);
            }
            else
            {
                return Ok($"работник id = {id} не найден");
            }
        }

        //получить всех работников
        [HttpGet("all")]
        public IActionResult GetEmployeeAll()
        {
            List<EmployeeResponse> response = new List<EmployeeResponse>();
            var done = false;
            var message = "";
            
            try
            {
                var employees = _dictionaryGlobal.employees;                
                foreach (var employee in employees)
                {
                    //переписываем задачи для response
                    List<int> tasks = new List<int>();
                    foreach (var task in employee.Tasks)
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
                done = true;
            }
            catch(Exception ex)
            {
                message = $"не удалось получить список работников, ошибка {ex.Message}";
            }

            if (done)
            {
                return Ok(response);
            }
            else
            {
                return Ok(message);
            }
        }

        //назначить задачу работнику
        [HttpPost("{id}/customer/{customerid}/contract/{contractid}/task/{taskid}")]
        public IActionResult RegisterTaskToEmployee([FromRoute] int id, [FromRoute] int customerid, [FromRoute] int contractid, [FromRoute] int taskid)
        {
            var customers = _dictionaryGlobal.customers;
            var employees = _dictionaryGlobal.employees;
            CustomerDto currentCustomer = null;
            ContractDto currentContract = null;
            TaskDto currentTask = null;
            EmployeeDto currentEmployee = null;
            TaskEmployeeResponse response = null;
            var find = false;
            var done = false;
            var message = "";

            foreach(var employee in employees)
            {
                if (employee.Id.Equals(id))
                {
                    //нашли работника
                    find = true;
                    currentEmployee = employee;
                    break;
                }
            }

            if (find)
            {
                //ищем задачу
                find = false;
                foreach(var customer in customers)
                {
                    if (customer.Id.Equals(customerid))
                    {
                        //найден заказчик
                        find = true;
                        currentCustomer = customer;
                        break;
                    }
                }

                if (find)
                {
                    //ищем контракт
                    find = false;
                    var contracts = currentCustomer.Contracts;
                    foreach(var contract in contracts)
                    {
                        if (contract.Id.Equals(contractid))
                        {
                            //нашли контракт
                            find = true;
                            currentContract = contract;
                            break;
                        }
                    }

                    if (find)
                    {
                        //ищем задачу
                        find = false;
                        var tasks = currentContract.Tasks;
                        foreach(var task in tasks)
                        {
                            if (task.Id.Equals(taskid))
                            {
                                //нашли задачу
                                find = true;
                                currentTask = task;
                                break;
                            }
                        }

                        if (find)
                        {
                            //проверяем, привязан ли работник к задаче
                            find = false;
                            var taskEmployees = currentTask.Employees;
                            foreach(var taskEmployee in taskEmployees)
                            {
                                if (taskEmployee.Employee.Id.Equals(currentEmployee.Id)){
                                    //работник уже привязан к задаче
                                    find = true;
                                    break;
                                }
                            }

                            if (!find)
                            {
                                try
                                {
                                    //привязываем работника к задаче
                                    TaskEmployeeDto taskEmployee = new TaskEmployeeDto()
                                    {
                                        Id = _dictionaryGlobal._taskEmployeeId,
                                        Done = false,
                                        Hours = 0.0,
                                        Task = currentTask,
                                        Employee = currentEmployee
                                    };

                                    currentTask.Employees.Add(taskEmployee);
                                    currentEmployee.Tasks.Add(currentTask);
                                    _dictionaryGlobal._taskEmployeeId++;

                                    //создаем response
                                    response = new TaskEmployeeResponse()
                                    {
                                        Id = taskEmployee.Id,
                                        Done = taskEmployee.Done,
                                        Hours = taskEmployee.Hours,
                                        Task = taskEmployee.Task.Id,
                                        Employee = taskEmployee.Employee.Id
                                    };
                                    done = true;
                                }
                                catch(Exception ex)
                                {
                                    message = $"не удалось прикрепить работника id = {id} к задаче taskId = {taskid}, ошибка {ex.Message}";
                                }
                            }
                            else
                            {
                                message = $"работник id = {currentEmployee.Id} уже прикреплен к задаче taskId = {currentTask.Id} контракта contractId = {currentContract.Id}, заказчика customerId = {currentCustomer.Id}";
                            }
                        }
                        else
                        {
                            message = $"задача id = {taskid} не найдена для контракта contractId = {contractid} у заказчика customerId = {customerid}";
                        }
                    }
                    else
                    {
                        message = $"контракт id = {contractid} не найден у заказчика customerId = {customerid}";
                    }
                } else
                {
                    message = $"заказчик id = {customerid} не найден";
                }
            } else
            {
                message = $"работник id = {id} не найден";
            }

            if (done)
            {
                return Ok(response);
            }
            else
            {
                return Ok(message);
            }
        }

        //посмотреть задачи работника
        [HttpGet("{id}/task/all")]
        public IActionResult GetEmployeeTasks([FromRoute] int id)
        {
            var employees = _dictionaryGlobal.employees;
            List<TaskResponse> response = null;
            EmployeeDto currentEmployee = null;
            var message = "";
            var find = false;
            var done = false;

            foreach(var employee in employees)
            {
                if (employee.Id.Equals(id))
                {
                    //нашли работника
                    find = true;
                    currentEmployee = employee;
                    break;
                }
            }

            if (find)
            {
                //смотрим задачи работника
                try
                {
                    var tasks = currentEmployee.Tasks;
                    response = new List<TaskResponse>();
                    foreach (var task in tasks)
                    {
                        //добавляем id работников, прикрепленных к задаче
                        List<int> taskEmployees = new List<int>();
                        foreach (var taskEmployee in task.Employees)
                        {
                            taskEmployees.Add(taskEmployee.Employee.Id);
                        }

                        response.Add(new TaskResponse()
                        {
                            Id = task.Id,
                            Contract = task.Contract.Id,
                            Employees = taskEmployees
                        });                        
                    }
                    done = true;
                }
                catch(Exception ex)
                {
                    message = $"не удалось получить список задач работника id = {id}, ошибка {ex.Message}";
                }
            }
            else
            {
                message = $"работник id = {id} не найден";
            }

            if (done)
            {
                return Ok(response);
            }
            else
            {
                return Ok(message);
            }
        }

        //выполнить задание
        [HttpPost("{id}/customer/{customerid}/contract/{contractid}/task/{taskid}/hours/{nhours}/done")]
        public IActionResult SetTaskIsDone([FromRoute] int id, [FromRoute] int customerid, [FromRoute] int contractid, [FromRoute] int taskid, [FromRoute] double nhours)
        {
            var employees = _dictionaryGlobal.employees;
            var customers = _dictionaryGlobal.customers;
            TaskEmployeeResponse response = null;
            EmployeeDto currentEmployee = null;
            CustomerDto currentCustomer = null;
            ContractDto currentContract = null;
            TaskDto currentTask = null;
            var find = false;
            var done = false;
            var message = "";
            
            //поиск работника
            foreach(var employee in employees)
            {
                if (employee.Id.Equals(id))
                {
                    //работник найден
                    find = true;
                    currentEmployee = employee;
                    break;
                }
            }

            if (find)
            {
                //поиск заказчика
                find = false;
                foreach(var customer in customers)
                {
                    if (customer.Id.Equals(customerid))
                    {
                        //заказчик найден
                        find = true;
                        currentCustomer = customer;
                        break;
                    }
                }

                if (find)
                {
                    //поиск контракта
                    find = false;
                    var contracts = currentCustomer.Contracts;
                    foreach(var contract in contracts)
                    {
                        if (contract.Id.Equals(contractid))
                        {
                            //контракт найден
                            find = true;
                            currentContract = contract;
                            break;
                        }
                    }

                    if (find)
                    {
                        //поиск задачи
                        find = false;
                        var tasks = currentContract.Tasks;
                        foreach(var task in tasks)
                        {
                            if (task.Id.Equals(taskid))
                            {
                                //нашли задачу
                                find = true;
                                currentTask = task;
                                break;
                            }
                        }

                        if (find)
                        {
                            //выполняем задачу
                            var taskEmployees = currentTask.Employees;
                            find = false;
                            foreach(var taskEmployee in taskEmployees)
                            {
                                if (taskEmployee.Employee.Id.Equals(id))
                                {
                                    //найдено соответствие задачи и работника
                                    if (!taskEmployee.Done)
                                    {
                                        try
                                        {
                                            taskEmployee.Done = true;
                                            taskEmployee.Hours = nhours;
                                            //создание инвойса по выполненной задаче
                                            var invoiceId = _dictionaryGlobal._invoiceid;
                                            var invoice = new InvoiceDto()
                                            {
                                                Id = invoiceId,
                                                Contract = currentContract,
                                                Task = currentTask,
                                                Cost = currentEmployee.Rate * taskEmployee.Hours,
                                                PayDone = false
                                            };

                                            //снимает деньги за работу у заказчика
                                            currentCustomer.BankAccount -= invoice.Cost;
                                            //зачисляем деньги работнику
                                            currentEmployee.BankAccount += invoice.Cost;
                                            invoice.PayDone = true;
                                            currentContract.Invoices.Add(invoice);
                                            _dictionaryGlobal._invoiceid++;

                                            //подготвка response
                                            response = new TaskEmployeeResponse()
                                            {
                                                Id = taskEmployee.Id,
                                                Employee = taskEmployee.Employee.Id,
                                                Task = taskEmployee.Task.Id,
                                                Hours = taskEmployee.Hours,
                                                Done = taskEmployee.Done
                                            };
                                            done = true;
                                        }
                                        catch(Exception ex)
                                        {
                                            message = $"Задача id = {taskid} не выполнена работником employeeId = {id}, ошибка {ex.Message}";
                                        }                                   
                                    }
                                    else
                                    {
                                        //это задание уже выполнено
                                        find = true;
                                        message = $"задание id = {taskid} уже выполнено у работника employeeId = {id}";
                                    }
                                    break;
                                }
                            }

                            if (!find)
                            {
                                message = $"у работника id = {id} не найдено задачи taskId = {taskid}";
                            }
                        }
                        else
                        {
                            message = $"задача id = {taskid} не найдена у контракта contractId = {contractid}, заказчика customerId = {customerid}";
                        }
                    }
                    else
                    {
                        message = $"контракт id = {contractid} не найден у заказчика customerId = {customerid}";
                    }
                }
                else
                {
                    message = $"заказчик id = {customerid} не найден";
                }
            }
            else
            {
                message = $"работник id = {id} не найден";
            }
            
            if (done)
            {
                return Ok(response);
            }
            else
            {
                return Ok(message);
            }
        }

        //посмотреть статус задания
        [HttpGet("{id}/customer/{customerid}/contract/{contractid}/task/{taskid}/status")]
        public IActionResult GetTaskStatus([FromRoute] int id, [FromRoute] int customerid, [FromRoute] int contractid, [FromRoute] int taskid)
        {
            var employees = _dictionaryGlobal.employees;
            var customers = _dictionaryGlobal.customers;
            TaskEmployeeResponse response = null;
            EmployeeDto currentEmployee = null;
            CustomerDto currentCustomer = null;
            ContractDto currentContract = null;
            TaskDto currentTask = null;
            var find = false;
            var done = false;
            var message = "";

            //поиск работника
            foreach (var employee in employees)
            {
                if (employee.Id.Equals(id))
                {
                    //работник найден
                    find = true;
                    currentEmployee = employee;
                    break;
                }
            }

            if (find)
            {
                //поиск заказчика
                find = false;
                foreach (var customer in customers)
                {
                    if (customer.Id.Equals(customerid))
                    {
                        //заказчик найден
                        find = true;
                        currentCustomer = customer;
                        break;
                    }
                }

                if (find)
                {
                    //поиск контракта
                    find = false;
                    var contracts = currentCustomer.Contracts;
                    foreach (var contract in contracts)
                    {
                        if (contract.Id.Equals(contractid))
                        {
                            //контракт найден
                            find = true;
                            currentContract = contract;
                            break;
                        }
                    }

                    if (find)
                    {
                        //поиск задачи
                        find = false;
                        var tasks = currentContract.Tasks;
                        foreach (var task in tasks)
                        {
                            if (task.Id.Equals(taskid))
                            {
                                //нашли задачу
                                find = true;
                                currentTask = task;
                                break;
                            }
                        }

                        if (find)
                        {
                            //ищем соответствие задачи и работника
                            var taskEmployees = currentTask.Employees;
                            find = false;
                            foreach (var taskEmployee in taskEmployees)
                            {
                                if (taskEmployee.Employee.Id.Equals(id))
                                {
                                    //найдено, подготовка response
                                    find = true;
                                    response = new TaskEmployeeResponse()
                                    {
                                        Id = taskEmployee.Id,
                                        Employee = taskEmployee.Employee.Id,
                                        Task = taskEmployee.Task.Id,
                                        Hours = taskEmployee.Hours,
                                        Done = taskEmployee.Done
                                    };
                                    done = true;                                    
                                    break;
                                }
                            }

                            if (!find)
                            {
                                message = $"у работника id = {id} не найдено задачи taskId = {taskid}";
                            }
                        }
                        else
                        {
                            message = $"задача id = {taskid} не найдена у контракта contractId = {contractid}, заказчика customerId = {customerid}";
                        }
                    }
                    else
                    {
                        message = $"контракт id = {contractid} не найден у заказчика customerId = {customerid}";
                    }
                }
                else
                {
                    message = $"заказчик id = {customerid} не найден";
                }
            }
            else
            {
                message = $"работник id = {id} не найден";
            }

            if (done)
            {
                return Ok(response);
            }
            else
            {
                return Ok(message);
            }
        }

        //посмотреть счет работника
        [HttpGet("{id}/bankaccount")]
        public IActionResult GetEmployeeBankAccount([FromRoute] int id)
        {
            var employees = _dictionaryGlobal.employees;
            EmployeeResponse response = null;
            var find = false;
            var done = false;
            var message = "";

            foreach (var employee in employees)
            {
                if (employee.Id.Equals(id))
                {
                    //нашли работника
                    find = true;
                    //формируем список задач работника для response
                    try
                    {
                        List<int> tasks = new List<int>();
                        foreach (var task in employee.Tasks)
                        {
                            tasks.Add(task.Id);
                        }

                        response = new EmployeeResponse()
                        {
                            Id = employee.Id,
                            BankAccount = employee.BankAccount,
                            Rate = employee.Rate,
                            Tasks = tasks
                        };
                        done = true;
                    }
                    catch(Exception ex)
                    {
                        message = $"не удалось сформировать ответ на запрос, ошибка {ex.Message}";
                    }
                }
            }

            if (!find)
            {
                if (message.Length == 0)
                {
                    message = $"работник id = {id} не найден";
                }
            }

            if (done)
            {
                return Ok(response);
            }
            else
            {
                return Ok(message);
            }
        }
    }
}