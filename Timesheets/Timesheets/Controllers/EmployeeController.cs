using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Requests;
using Timesheets.DataAccessLayer.Models;
using Timesheets.DataAccessLayer.Services.Interfaces;

namespace Timesheets.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private ICustomerService _customerService;
        private IContractService _contractService;
        private ITaskService _taskService;
        private ITaskEmployeeService _taskEmployeeService;
        private IInvoiceService _invoiceService;
        private IEmployeeService _employeeService;

        public EmployeeController(
            ILogger<EmployeeController> logger,
            ICustomerService customerService,
            IContractService contractService,
            ITaskService taskService,
            ITaskEmployeeService taskEmployeeService,
            IInvoiceService invoiceService,
            IEmployeeService employeeService
            )
        {
            _logger = logger;
            _customerService = customerService;
            _contractService = contractService;
            _taskService = taskService;
            _taskEmployeeService = taskEmployeeService;
            _invoiceService = invoiceService;
            _employeeService = employeeService;
        }

        //зарегистрировать работника
        [HttpPost("rate/{rate}/register")]
        public IActionResult RegisterEmployee([FromRoute] double rate)
        {
            _logger.LogInformation("RegisterEmployee() запуск метода");
            EmployeeModel response = null;
            var message = "";
            var done = false;

            try
            {
                EmployeeRequest employeeRequest = new EmployeeRequest()
                {
                    BankAccount = 0,
                    Rate = rate
                };                

                response = _employeeService.RegisterEmployee(employeeRequest);
                if (response != null)
                {
                    done = true;
                }
                else
                {
                    message = $"добавить работника не удалось";
                }
            }
            catch (Exception ex)
            {
                message = $"добавить работника не удалось, ошибка {ex.Message}";
            }

            if (done)
            {
                _logger.LogInformation("RegisterEmployee() завершено");
                return Ok(response);
            }
            else
            {
                _logger.LogError($"RegisterEmployee() ошибка, {message}");
                return Ok(message);
            }
        }

        //получить работника по id
        [HttpGet("{employeeid}")]
        public IActionResult GetEmployee([FromRoute] int employeeid)
        {
            _logger.LogInformation("GetEmployee() запуск метода");
            EmployeeModel response = _employeeService.GetEmployee(employeeid);
            if(response != null)
            {
                //работник найден
                return Ok(response);
            }
            else
            {
                var message = $"работник id = {employeeid} не найден";
                _logger.LogError($"GetEmployee() ошибка, {message}");
                return Ok(message);
            }
        }

        //получить всех работников
        [HttpGet("all")]
        public IActionResult GetEmployeeAll()
        {
            _logger.LogInformation("GetEmployeeAll() запуск метода");
            List<EmployeeModel> response = _employeeService.GetEmployeeAll();
            _logger.LogInformation("GetEmployeeAll() завершено");
            return Ok(response);
        }

        //назначить задачу работнику
        [HttpPost("customer/{customerid}/contract/{contractid}/registerTaskToEmployee")]
        public IActionResult RegisterTaskToEmployee([FromRoute] int customerid, [FromRoute] int contractid, [FromBody] TaskEmployeeRequest taskEmployeeDto)
        {
            _logger.LogInformation("RegisterTaskToEmployee() запуск метода");
            var message = "";
            var find = false;
            var done = false;

            TaskEmployeeModel response = null;
            //работник
            EmployeeModel employeeModel = _employeeService.GetEmployee(taskEmployeeDto.Employee);
            if (employeeModel != null)
            {
                //заказчик
                CustomerModel customerModel = _customerService.GetCustomer(customerid);
                if (customerModel != null)
                {
                    //контракт
                    ContractModel contractModel = _contractService.GetContract(customerid, contractid);
                    if (contractModel != null)
                    {
                        //задача
                        TaskModel taskModel = _taskService.GetTask(customerid, contractid, taskEmployeeDto.Task);
                        if (taskModel != null)
                        {
                            //проверяем, привязан ли работник к задаче
                            var taskEmployees = taskModel.TaskEmployees;
                            if (taskEmployees != null)
                            {
                                foreach (var taskEmployee in taskEmployees)
                                {
                                    var taskEmployeeModel = _taskEmployeeService.GetTaskEmployee(taskEmployeeDto.Task, taskEmployeeDto.Employee);
                                    if (taskEmployeeModel != null)
                                    {
                                        find = true;
                                        break;
                                    }
                                }
                            }
                            
                            if (!find)
                            {
                                //привязываем работника к задаче
                                TaskEmployeeRequest taskEmployeeRequest = new TaskEmployeeRequest()
                                {
                                    Employee = taskEmployeeDto.Employee,
                                    Task = taskEmployeeDto.Task
                                };

                                response = _taskEmployeeService.RegisterTaskEmployee(taskEmployeeRequest);
                                if (response != null)
                                {
                                    done = true;
                                }
                                else
                                {
                                    message = $"не удалось прикрепить работника id = {taskEmployeeDto.Employee} к задаче taskId = {taskEmployeeDto.Task}";
                                }
                            }
                            else
                            {
                                message = $"работник id = {taskEmployeeDto.Employee} уже привязан к задаче taskId = {taskEmployeeDto.Task} контракта contractId = {contractid}";
                            }
                        }
                        else
                        {
                            message = $"задача id = {taskEmployeeDto.Task} не найдена у заказчика customerId = {customerid}, контракт contractId = {contractid}";
                        }
                    }
                    else
                    {
                        message = $"контракт id = {contractid}, заказчика customerId = {customerid} не найден";
                    }
                }
                else
                {
                    message = $"заказчик id = {customerid} не найден";
                }
            }
            else
            {
                message = $"работник id = {taskEmployeeDto.Employee} не найден";
            }

            if (done)
             {
                _logger.LogInformation("RegisterTaskToEmployee() завершено");
                return Ok(response);
             }
             else
             {
                _logger.LogError($"RegisterTaskToEmployee() ошибка, {message}");
                return Ok(message);
             }
        }

        //посмотреть задачи работника
        [HttpGet("{employeeid}/task/all")]
        public IActionResult GetEmployeeTasks([FromRoute] int employeeid)
        {
            _logger.LogInformation("GetEmployeeTasks() запуск метода");
            var message = "";
            var done = false;
            List<TaskEmployeeModel> taskEmployeeResponse = null;

            EmployeeModel employeeResponse = _employeeService.GetEmployee(employeeid);
            if (employeeResponse != null)
            {
                //работник существует
                taskEmployeeResponse = _taskEmployeeService.GetTaskEmployeeAll(new EmployeeDto() { Id = employeeid });
                if (taskEmployeeResponse != null)
                {
                    if (taskEmployeeResponse.Count > 0)
                    {
                        done = true;
                    }
                    else
                    {
                        message = $"у работника id = {employeeid} нет задач";
                    }
                }
                else
                {
                    message = "в результате запроса произошла ошибка";
                }
            }
            else
            {
                message = $"работник id = {employeeid} не существует";
            }

            if (done)
            {
                _logger.LogInformation("GetEmployeeTasks() завершено");
                return Ok(taskEmployeeResponse);
            }
            else
            {
                _logger.LogError($"GetEmployeeTasks() ошибка, {message}");
                return Ok(message);
            }
        }

        //выполнить задание
        [HttpPost("{employeeid}/task/{taskid}/hours/{nhours}/taskDone")]
        public IActionResult SetTaskIsDone([FromRoute] int employeeid, [FromRoute] int taskid, [FromRoute] double nhours)
        {
            _logger.LogInformation("SetTaskIsDone() запуск метода");
            var done = false;
            var message = "";
            //
            TaskEmployeeModel response = null;
            EmployeeModel employee = _employeeService.GetEmployee(employeeid);
            if (employee != null)
            {
                //работник найден
                var taskEmployees = _taskEmployeeService.GetTaskEmployeeAll(new EmployeeDto() { Id = employeeid });
                if (taskEmployees != null)
                {
                    foreach (var taskEmployee in taskEmployees)
                    {
                        if (taskEmployee.EmployeeId == employeeid &&
                            taskEmployee.TaskId == taskid)
                        {
                            //найдена нужная задача
                            taskEmployee.Done = true;
                            taskEmployee.Hours = nhours;
                            //сохранение данных
                            if (_taskEmployeeService.SetTaskEmployee(taskEmployee))
                            {
                                response = taskEmployee;
                                //
                                var task = _taskService.GetTask(taskid);
                                if (task != null)
                                {
                                    //получить контракт
                                    var contract = _contractService.GetContract(task.ContractId);
                                    if (contract != null)
                                    {
                                        var customer = _customerService.GetCustomer(contract.CustomerId);
                                        if (customer != null)
                                        {
                                            //создание инвойса по выполненной задаче
                                            InvoiceRequest invoiceRequest = new InvoiceRequest()
                                            {
                                                Contract = task.ContractId,
                                                Task = taskEmployee.TaskId,
                                                Cost = employee.Rate * nhours
                                            };

                                            var invoice = _invoiceService.RegisterInvoice(invoiceRequest);
                                            if (invoice != null)
                                            {
                                                //снимает деньги за работу у заказчика
                                                customer.BankAccount -= invoice.Cost;
                                                //зачисляем деньги работнику
                                                employee.BankAccount += invoice.Cost;
                                                invoice.PayDone = true;
                                                //сохранение измененных данных
                                                if (_customerService.SetCustomer(customer) &&
                                                _employeeService.SetEmployee(employee) &&
                                                _invoiceService.SetInvoice(customer.Id, invoice))
                                                {
                                                    done = true;
                                                }
                                                break;
                                            }
                                            else
                                            {
                                                message = $"счет не зарегистрирован";
                                            }
                                        }
                                        else
                                        {
                                            message = $"заказчик {contract.CustomerId} не найден";
                                        }
                                    }
                                    else
                                    {
                                        message = $"контракт {contract.Id} заказчик {contract.CustomerId} не найден";
                                    }
                                }
                                else
                                {
                                    message = $"задача taskid = {taskid} не найдена";
                                }
                            }
                            else
                            {
                                message = $"данные не сохранены, действие отменено";
                            }
                        }
                    }
                }
            }
            else
            {
                message = $"работник id = {employeeid} не найден";
            }

            if (done)
            {
                _logger.LogInformation("SetTaskIsDone() завершено");
                return Ok(response);
            }
            else
            {
                _logger.LogError($"SetTaskIsDone() ошибка, {message}");
                return Ok(message);
            }
        }

        //посмотреть статус задания
        [HttpGet("{employeeid}/task/{taskid}/status")]
        public IActionResult GetTaskStatus([FromRoute] int employeeid, [FromRoute] int taskid)
        {
            _logger.LogInformation("GetTaskStatus() запуск метода");
            var done = false;
            var message = "";
            TaskEmployeeModel response = null;
            EmployeeModel employee = _employeeService.GetEmployee(employeeid);
            if (employee != null)
            {
                //работник найден
                var taskEmployees = _taskEmployeeService.GetTaskEmployeeAll(taskid);
                if (taskEmployees != null)
                {
                    if (taskEmployees.Count > 0)
                    {
                        foreach (var taskEmployee in taskEmployees)
                        {
                            if (taskEmployee.EmployeeId == employeeid &&
                                taskEmployee.TaskId == taskid)
                            {
                                //работник и задание совпадает
                                response = taskEmployee;
                                done = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        message = $"у работника id = {employeeid} не найдена задача {taskid}";
                    }
                }
            }
            else
            {
                message = $"работник id = {employeeid} не найден";
            }

            if (done)
            {
                _logger.LogInformation("GetTaskStatus() завершено");
                return Ok(response);
            }
            else
            {
                _logger.LogError($"GetTaskStatus() ошибка, {message}");
                return Ok(message);
            }
        }

        //посмотреть счет работника
        [HttpGet("{employeeid}/bankaccount")]
        public IActionResult GetEmployeeBankAccount([FromRoute] int employeeid)
        {
            _logger.LogInformation("GetEmployeeBankAccount() запуск метода");
            var done = false;
            var message = "";
            //
            EmployeeModel response = _employeeService.GetEmployee(employeeid);
            if (response != null)
            {
                //работник найден
                done = true;
            }
            else
            {
                message = $"работник id = {employeeid} не найден";
            }

            if (done)
            {
                _logger.LogInformation("GetEmployeeBankAccount() завершено");
                return Ok(response);
            }
            else
            {
                _logger.LogError($"GetEmployeeBankAccount() ошибка, {message}");
                return Ok(message);
            }
        }
    }
}