using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Responses;
using Timesheets.DataAccessLayer.Models;
using Timesheets.DataAccessLayer.Services.Interfaces;

namespace Timesheets.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly DictionariesGlobal _dictionaryGlobal;
        private IEmployeeService _employeeService;
        private ICustomerService _customerService;
        private IContractService _contractService;
        private ITaskService _taskService;
        private IInvoiceService _invoiceService;

        public EmployeeController(
            DictionariesGlobal dictionariesGlobal,
            IEmployeeService employeeService,
            ICustomerService customerService,
            IContractService contractService,
            ITaskService taskService,
            IInvoiceService invoiceService)
        {
            _dictionaryGlobal = dictionariesGlobal;
            _employeeService = employeeService;
            _customerService = customerService;
            _contractService = contractService;
            _taskService = taskService;
            _invoiceService = invoiceService;
        }

        //зарегистрировать работника
        [HttpPost("rate/{rate}")]
        public IActionResult RegisterEmployee([FromRoute] double rate)
        {
            EmployeeResponse response = null;
            var employeeId = _dictionaryGlobal._employeeid;
            var message = "";
            var done = false;

            try
            {
                EmployeeModel employee = new EmployeeModel()
                {
                    Id = employeeId,
                    BankAccount = 0,
                    Rate = rate,
                    Tasks = new List<TaskModel>()
                };

                response = _employeeService.RegisterEmployee(employee);
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
            EmployeeResponse response = _employeeService.GetEmployee(id);
            if(response != null)
            {
                //работник найден
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
            List<EmployeeResponse> response = _employeeService.GetEmployeeAll();
            return Ok(response);                
        }

        //назначить задачу работнику
        [HttpPost("{id}/customer/{customerid}/contract/{contractid}/task/{taskid}")]
        public IActionResult RegisterTaskToEmployee(
            [FromRoute] int id, 
            [FromRoute] int customerid, 
            [FromRoute] int contractid, 
            [FromRoute] int taskid)
        {
            var message = "";
            var find = false;
            var done = false;
            TaskEmployeeResponse response = null;
            int taskEmployeeId = _dictionaryGlobal._taskEmployeeId;
            //работник найден, ищем задачу
            EmployeeModel employeeModel = _employeeService.GetModel(_employeeService.GetEmployee(id));
            if (employeeModel != null)
            {
                //заказчик найден
                CustomerModel customer = _customerService.GetModel(_customerService.GetCustomer(customerid));
                if (customer != null)
                {
                    //контракт найден, получаем объект
                    ContractModel contractModel = _contractService.GetModel(_contractService.GetContract(customerid, contractid));
                    if (contractModel != null)
                    {
                        //задача найдена, получаем объект
                        TaskModel taskModel = _taskService.GetModel(customerid, contractid, taskid);
                        if (taskModel != null)
                        {
                            //проверяем, привязан ли работник к задаче
                            var taskEmployees = taskModel.Employees;
                            foreach (var taskEmployee in taskEmployees)
                            {
                                if (taskEmployee.Employee.Id.Equals(employeeModel.Id))
                                {
                                    //работник уже привязан
                                    find = true;
                                    break;
                                }
                            }

                            if (!find)
                            {
                                //привязываем работника к задаче
                                TaskEmployeeModel taskEmployeeModel = new TaskEmployeeModel()
                                {
                                    Id = taskEmployeeId,
                                    Done = false,
                                    Hours = 0.0,
                                    Task = taskModel,
                                    Employee = employeeModel
                                };

                                response = _employeeService.RegisterTaskToEmployee(employeeModel, taskModel, taskEmployeeModel);
                                if (response != null)
                                {
                                    //получили response
                                    done = true;
                                }
                                else
                                {
                                    message = $"не удалось прикрепить работника id = {id} к задаче taskId = {taskid}";
                                }
                            }
                            else
                            {
                                message = $"работник id = {id} уже привязан к задаче taskId = {taskid} контракта contractId = {contractid}";
                            }
                        }
                        else
                        {
                            message = $"задача id = {taskid} не найдена у заказчика customerId = {customerid}, контракт contractId = {contractid}";
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
            var done = false;
            var message = "";
            List<TaskResponse> response = null;
            EmployeeModel employee = _employeeService.GetModel(_employeeService.GetEmployee(id));
            if (employee != null)
            {
                //работник найден, получаем задачи
                response = _employeeService.GetTaskAll(employee);
                done = true;
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
        public IActionResult SetTaskIsDone(
            [FromRoute] int id,
            [FromRoute] int customerid,
            [FromRoute] int contractid, 
            [FromRoute] int taskid,
            [FromRoute] double nhours)
        {
            var done = false;
            var message = "";
            TaskEmployeeResponse response = null;
            EmployeeModel employee = _employeeService.GetModel(_employeeService.GetEmployee(id));
            if (employee != null)
            {
                //работник найден
                CustomerModel customer = _customerService.GetModel(_customerService.GetCustomer(customerid));
                if (customer != null)
                {
                    //заказчик найден
                    ContractModel contract = _contractService.GetModel(_contractService.GetContract(customerid, contractid));
                    if (contract != null)
                    {
                        //контракт найден
                        TaskModel task = _taskService.GetModel(customerid, contractid, taskid);
                        if (task != null)
                        {
                            //задача найдена, выполняем задание                            
                            foreach (var taskEmployee in task.Employees)
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
                                            InvoiceModel invoice = new InvoiceModel()
                                            {
                                                Id = invoiceId,
                                                Contract = contract,
                                                Task = task,
                                                Cost = employee.Rate * taskEmployee.Hours,
                                                PayDone = false
                                            };

                                            //снимает деньги за работу у заказчика
                                            customer.BankAccount -= invoice.Cost;
                                            //зачисляем деньги работнику
                                            employee.BankAccount += invoice.Cost;
                                            invoice.PayDone = true;
                                            InvoiceResponse invoiceResponse = _invoiceService.RegisterInvoice(invoice);

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
                                        catch (Exception ex)
                                        {
                                            message = $"задача id = {taskid} не выполнена работником employeeId = {id}, ошибка {ex.Message}";
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        message = $"задача id = {taskid} уже выполнена у работника employeeId = {id}";
                                        break;
                                    }
                                }
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
        public IActionResult GetTaskStatus(
            [FromRoute] int id,
            [FromRoute] int customerid,
            [FromRoute] int contractid, 
            [FromRoute] int taskid)
        {
            var done = false;
            var message = "";
            TaskEmployeeResponse response = null;
            EmployeeModel employee = _employeeService.GetModel(_employeeService.GetEmployee(id));
            if (employee != null)
            {
                //работник найден
                CustomerModel customer = _customerService.GetModel(_customerService.GetCustomer(customerid));
                if (customer != null)
                {
                    //заказчик найден
                    ContractModel contract = _contractService.GetModel(_contractService.GetContract(customerid, contractid));
                    if (contract != null)
                    {
                        //контракт найден
                        TaskModel task = _taskService.GetModel(customerid, contractid, taskid);
                        if (task != null)
                        {
                            //задача найдена, получаем состояние
                            response = _employeeService.GetTaskEmployee(id, taskid);
                            {
                                if (response != null)
                                {
                                    done = true;
                                }
                                else
                                {
                                    message = $"не удалось получить задачу id = {taskid}";
                                }
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
            var done = false;
            var message = "";
            EmployeeResponse response = null;
            response = _employeeService.GetEmployee(id);
            if (response != null)
            {
                //работник найден
                done = true;
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
    }
}