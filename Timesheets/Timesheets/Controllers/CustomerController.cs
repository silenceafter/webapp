using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Requests;
using Timesheets.Data;
using Timesheets.DataAccessLayer.Interfaces;
using Timesheets.DataAccessLayer.Models;
using Timesheets.DataAccessLayer.Services.Interfaces;
using ILogger = NLog.ILogger;

namespace Timesheets.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private static ILogger<CustomerController> _logger;
        private ICustomerService _customerService;
        private IContractService _contractService;
        private ITaskService _taskService;
        private IInvoiceService _invoiseService;
        
        public CustomerController(
            ILogger<CustomerController> logger,
            ICustomerService customerService,
            IContractService contractService, 
            ITaskService taskService, 
            IInvoiceService invoiceService
            )
        {
            _logger = logger;
            _customerService = customerService;
            _contractService = contractService;
            _taskService = taskService;
            _invoiseService = invoiceService;   
        }

        //регистрация, действие по умолчанию
        [HttpPost]
        public IActionResult RegisterCustomer()
        {
            _logger.LogInformation("RegisterCustomer() запуск метода");
            CustomerModel response = null;                        
            var message = "";
            var done = false;

            try
            {
                CustomerRequest customerRequest = new CustomerRequest()
                {
                    BankAccount = 100000.0
                };

                response = _customerService.RegisterCustomer(customerRequest);
                if (response != null)
                {
                    done = true;
                }
                else
                {
                    message = $"добавить заказчика не удалось";
                }
            }
            catch (Exception ex)
            {
                message = $"добавить заказчика не удалось, ошибка {ex.Message}";
            }

            if (done)
            {
                _logger.LogInformation("RegisterCustomer() завершено");
                return Ok(response);
            }
            else
            {
                _logger.LogError($"RegisterCustomer() ошибка, {message}");
                return Ok(message);
            }
        }

        //получить по id
        [HttpGet("{customerid}")]
        public IActionResult GetCustomer([FromRoute] int customerid)
        {
            _logger.LogInformation("GetCustomer() запуск метода");
            CustomerModel response = null;
            response = _customerService.GetCustomer(customerid);

            if (response != null)
            {
                _logger.LogInformation("GetCustomer() завершено");
                return Ok(response);
            }
            else
            {
                var message = $"заказчик id = {customerid} не найден";
                _logger.LogError($"GetCustomer() ошибка, {message}");
                return Ok(message);
            }
        }

        //получить всех закачиков
        [HttpGet("all")]
        public IActionResult GetCustomerAll()
        {
            _logger.LogInformation("GetCustomerAll() запуск метода");
            List<CustomerModel> response = _customerService.GetCustomerAll();
            _logger.LogInformation("GetCustomerAll() завершено");
            return Ok(response);
        }

        //зарегистрировать контракт по customer id
        [HttpPost("{customerid}/contract")]
        public IActionResult RegisterContract([FromRoute] int customerid)
        {
            _logger.LogInformation("RegisterContract() запуск метода");
            ContractModel response = null;
            var message = "";
            var done = false;

            try
            {
                //ищем заказчика
                var customer = _customerService.GetCustomer(customerid);
                if (customer != null)
                {
                    //заказчик найден
                    //есть такой контракт?
                    ContractRequest contractRequest = new ContractRequest()
                    {                            
                        Customer = customer.Id
                    };

                    response = _contractService.RegisterContract(contractRequest);
                    if (response != null)
                    {
                        done = true;
                    }
                    else
                    {
                        message = $"не удалось добавить контракт у заказчика id = {customerid}";
                    }
                } else
                {
                    message = $"заказчик id = {customerid} не найден";
                }                
            }
            catch (Exception ex)
            {
                message = $"не удалось добавить контракт у заказчика id = {customerid}, ошибка {ex.Message}";
            }

            if (done)
            {
                _logger.LogInformation("RegisterContract(), завершено");
                return Ok(response);
            }
            else
            {
                _logger.LogError($"RegisterContract() ошибка, {message}");
                return Ok(message);
            }
        }

        //получить контракт по id
        [HttpGet("{customerid}/contract/{contractid}")]
        public IActionResult GetContract([FromRoute] int customerid, [FromRoute] int contractid)
        {
            _logger.LogInformation("GetContract() запуск метода");
            var done = false;
            var message = "";

            ContractModel response = null;
            CustomerModel customer = _customerService.GetCustomer(customerid);
            if(customer != null)
            {
                //заказчик найден, ищем контракт
                response = _contractService.GetContract(customerid, contractid);
                if(response != null)
                {
                    //контракт найден
                    done = true;
                }
                else
                {
                    message = $"контракт id = {contractid} заказчика customerId = {customerid} не найден";
                }
            }
            else
            {
                message = $"заказчик id = {customerid} не найден";
            }

            if(done)
            {
                _logger.LogInformation("GetContract(), выполнено");
                return Ok(response);
            }
            else
            {
                _logger.LogError($"GetContract() ошибка, {message}");
                return Ok(message);
            }
        }

        //получить контракты по customer id
        [HttpGet("{customerid}/contract/all")]
        public IActionResult GetContractAll([FromRoute] int customerid)
        {
            _logger.LogInformation("GetContractAll() запуск метода");
            var message = "";
            var done = false;
            List<ContractModel> response = _contractService.GetContractAll(customerid);
            if(response != null)
            {
                if(response.Count > 0)
                {
                    done = true;
                }
                else
                {
                    message = $"у заказчика id = {customerid} нет контрактов";
                }
            }
            else
            {
                message = $"заказчик id = {customerid} не существует";
            }

            if (done)
            {
                _logger.LogInformation("GetContractAll(), завершено");
                return Ok(response);
            }
            else
            {
                _logger.LogError($"GetContractAll() ошибка, {message}");
                return Ok(message);
            }
        }

        //получить инвойс по task id
        [HttpGet("{customerid}/contract/{contractid}/task/{taskid}/invoice")]
        public IActionResult GetInvoice([FromRoute] int customerid, [FromRoute] int contractid, [FromRoute] int taskid)
        {
            _logger.LogInformation("GetInvoice() запуск метода");
            var done = false;
            var message = "";
            InvoiceModel response = null;

            //существует ли заказчик
            CustomerModel customerResponse = _customerService.GetCustomer(customerid);
            if(customerResponse != null)
            {
                //заказчик найден, существует ли контракт
                ContractModel contractResponse = _contractService.GetContract(customerid, contractid);
                if(contractResponse != null)
                {
                    //контракт существует, ищем задачу
                    TaskModel taskResponse = _taskService.GetTask(customerid, contractid, taskid);
                    if (taskResponse != null)
                    {
                        //задача существует, ищем инвойс
                        response = _invoiseService.GetInvoice(customerid, contractid, taskid);
                        if (response != null)
                        {
                            done = true;
                        }
                        else
                        {
                            message = $"счет id = {taskid} у заказчика customerId = {customerid}, контракт contractId = {contractid} не найдена";
                        }
                    }
                    else
                    {
                        message = $"задача id = {taskid} у заказчика customerId = {customerid}, контракт contractId = {contractid} не найдена";
                    }                       
                }
                else
                {
                    message = $"контракт id = {contractid} не существует у заказчика customerId = {customerid}";
                }
            }
            else
            {
                message = $"заказчик id = {customerid} не найден";
            }
            
            if (done)
            {
                _logger.LogInformation("GetInvoice(), завершено");
                return Ok(response);
            }
            else
            {
                _logger.LogError($"GetInvoice() ошибка, {message}");
                return Ok(message);
            }
        }

        //получить все инвойсы по контракту
        [HttpGet("{customerid}/contract/{contractid}/invoice/all")]
        public IActionResult GetInvoiceAll([FromRoute] int customerid, [FromRoute] int contractid)
        {
            _logger.LogInformation("GetInvoiceAll() запуск метода");
            var message = "";
            var done = false;
            List<InvoiceModel> invoiceResponse = null;

            CustomerModel customerResponse = _customerService.GetCustomer(customerid);
            if (customerResponse != null)
            {
                ContractModel contractResponse = _contractService.GetContract(customerid, contractid);
                if (contractResponse != null)
                {
                    //заказчик существует
                    invoiceResponse = _invoiseService.GetInvoiceAll(contractid);
                    if (invoiceResponse != null)
                    {
                        if (invoiceResponse.Count > 0)
                        {
                            done = true;
                        }
                        else
                        {
                            message = $"у заказчика id = {customerid}, контракт contractId = {contractid} нет счетов";
                        }
                    }
                }
                else
                {
                    message = $"контракт id = {contractid} у заказчика customerId = {customerid} не найден";
                }
            }
            else
            {
                message = $"заказчик id = {customerid} не существует";
            }

            if (done)
            {
                _logger.LogInformation("GetInvoiceAll() завершено");
                return Ok(invoiceResponse);
            }
            else
            {
                _logger.LogError($"GetInvoiceAll() ошибка, {message}");
                return Ok(message);
            }
        }

        //зарегистрировать задачу
        [HttpPost("{customerid}/contract/{contractid}/task")]
        public IActionResult RegisterTask([FromRoute] int customerid, [FromRoute] int contractid)
        {
            _logger.LogInformation("RegisterTask() запуск метода");
            TaskModel response = null;
            var message = "";
            var done = false;

            try
            {
                //ищем контракт
                ContractModel contract =  _contractService.GetContract(customerid, contractid);
                if(contract != null)
                {
                    //контракт найден
                    TaskRequest task = new TaskRequest()
                    {
                        Contract = contractid
                    };
                    
                    response = _taskService.RegisterTask(task);
                    if (response != null)
                    {
                        done = true;
                    }
                    else
                    {
                        message = $"не удалось добавить задачу контракту id = {contractid}, заказчик customerId = {customerid}";
                    }                     
                }
                else
                {
                    //проверяем, существует ли заказчик
                    CustomerModel customerResponse = _customerService.GetCustomer(customerid);
                    if(customerResponse == null)
                    {
                        //заказчик не существует
                        message = $"заказчик id = {customerid} не найден";
                    } 
                    else
                    {
                        message = $"контракт id = {contractid} не найден у заказчика customerId = {customerid}";
                    }
                }
             }
             catch (Exception ex)
             {
                 message = $"не удалось добавить задачу контракту id {contractid}, заказчик customerId = {customerid}, ошибка {ex.Message}";
             }

             if (done)
             {
                 _logger.LogInformation("GetInvoiceAll() завершено");
                 return Ok(response);
             }
             else
             {
                 _logger.LogError($"GetInvoiceAll() ошибка, {message}");
                 return Ok(message);
             }
        }

        //получить задачу по id
        [HttpGet("{customerid}/contract/{contractid}/task/{taskid}")]
        public IActionResult GetTask([FromRoute] int customerid, [FromRoute] int contractid, [FromRoute] int taskid)
        {
            _logger.LogInformation("GetTask() запуск метода");
            var done = false;
            var message = "";
            TaskModel response = null;
            response = _taskService.GetTask(customerid, contractid, taskid);

            if (response != null)
            {
                done = true;
            }
            else
            {
                //существует ли заказчик
                CustomerModel customerResponse = _customerService.GetCustomer(customerid);
                if(customerResponse != null)
                {
                    //заказчик найден
                    //существует ли контракт
                    ContractModel contractResponse = _contractService.GetContract(customerid, contractid);
                    if(contractResponse != null)
                    {
                        //контракт существует, но response = null
                        message = $"задача id = {taskid} у заказчика customerId = {customerid}, контракт contractId = {contractid} не найдена";
                    }
                    else
                    {
                        message = $"контракт id = {contractid} не существует у заказчика customerId = {customerid}";
                    }
                }
                else
                {
                    message = $"заказчик id = {customerid} не найден";
                }
            }
            
            if (done)
            {
                _logger.LogInformation("GetTask() завершено");
                return Ok(response);
            }
            else
            {
                _logger.LogError($"GetTask() ошибка, {message}");
                return Ok(message);
            }
        }

        //получить задачи по контракту
        [HttpGet("{customerid}/contract/{contractid}/task/all")]
        public IActionResult GetTaskAll([FromRoute] int customerid, [FromRoute] int contractid)
        {
            _logger.LogInformation("GetTaskAll() запуск метода");
            var message = "";
            var done = false;
            List<TaskModel> taskResponse = null;

            CustomerModel customerResponse = _customerService.GetCustomer(customerid);
            if (customerResponse != null)
            {
                ContractModel contractResponse = _contractService.GetContract(customerid, contractid);
                if (contractResponse != null)
                {
                    //контракт найден
                    taskResponse = _taskService.GetTaskAll(contractid);
                    if (taskResponse != null)
                    {
                        if (taskResponse.Count > 0)
                        {
                            done = true;
                        }
                        else
                        {
                            message = $"у заказчика id = {customerid}, контракт contractId = {contractid} нет задач";
                        }
                    }
                }
                else
                {
                    message = $"контракт id = {contractid} у заказчика customerId = {customerid} не найден";
                }
            }
            else
            {
                message = $"заказчик id = {customerid} не существует";
            }

            if (done)
            {
                _logger.LogInformation("GetTaskAll() завершено");
                return Ok(taskResponse);
            }
            else
            {
                _logger.LogError($"GetTaskAll() ошибка, {message}");
                return Ok(message);
            }
        }
    }
}

        