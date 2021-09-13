using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Responses;
using Timesheets.DataAccessLayer.Interfaces;
using Timesheets.DataAccessLayer.Models;
using Timesheets.DataAccessLayer.Services.Interfaces;

namespace Timesheets.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly DictionariesGlobal _dictionaryGlobal;
        private ICustomerService _customerService;
        private IContractService _contractService;
        private ITaskService _taskService;
        private IInvoiceService _invoiseService;

        public CustomerController(
            DictionariesGlobal dictionariesGlobal, 
            ICustomerService customerService, 
            IContractService contractService, 
            ITaskService taskService, 
            IInvoiceService invoiceService)
        {
            _dictionaryGlobal = dictionariesGlobal;
            _customerService = customerService;
            _contractService = contractService;
            _taskService = taskService;
            _invoiseService = invoiceService;
        }

        //регистрация, действие по умолчанию
        [HttpPost]
        public IActionResult RegisterCustomer()
        {
            CustomerResponse response = null;
            var customerId = _dictionaryGlobal._customerid;                        
            var message = "";
            var done = false;

            try
            {
                CustomerModel customer = new CustomerModel()
                {
                    Id = customerId,
                    Contracts = new List<ContractModel>(),
                    BankAccount = 100000.0
                };
                response = _customerService.RegisterCustomer(customer);
                if (response != null)
                {
                    done = true;
                }
                else
                {
                    message = $"добавить заказчика не удалось";
                }
            }
            catch(Exception ex)
            {
                message = $"добавить заказчика не удалось, ошибка {ex.Message}";
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

        //получить по id
        [HttpGet("{id}")]
        public IActionResult GetCustomer([FromRoute] int id)
        {
            CustomerResponse response = null;
            response = _customerService.GetCustomer(id);

            if(response != null)
            {
                return Ok(response);
            }
            else
            {
                return Ok($"заказчик id = {id} не найден");
            }                    
        }

        //получить всех закачиков
        [HttpGet("all")]
        public IActionResult GetCustomerAll()
        {
            List<CustomerResponse> customerResponse = _customerService.GetCustomerAll();
            return Ok(customerResponse);
        }

        //зарегистрировать контракт по customer id
        [HttpPost("{id}/contract")]
        public IActionResult RegisterContract([FromRoute] int id)
        {
            ContractResponse response = null;
            var contractId = _dictionaryGlobal._contractid;
            var message = "";
            var done = false;

            try
            {
                //ищем заказчика
                CustomerModel customer = null;
                CustomerResponse customerResponse = _customerService.GetCustomer(id);

                if (customerResponse != null)
                {
                    //заказчик найден
                    customer = _customerService.GetModel(customerResponse);
                    if(customer != null)
                    {
                        //объект найден
                        ContractModel contract = new ContractModel()
                        {
                            Id = contractId,
                            Customer = customer,
                            Tasks = new List<TaskModel>(),
                            Invoices = new List<InvoiceModel>()
                        };

                        response = _contractService.RegisterContract(contract);
                        if (response != null)
                        {
                            done = true;
                        }
                        else
                        {
                            message = $"не удалось добавить контракт у заказчика id = {id}";
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                message = $"не удалось добавить контракт у заказчика id = {id}, ошибка {ex.Message}";
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
       
        //получить контракт по id
        [HttpGet("{id}/contract/{contractid}")]
        public IActionResult GetContract([FromRoute] int id, [FromRoute] int contractid)
        {
            var done = false;
            var message = "";
            ContractResponse response = null;
            CustomerResponse customer = _customerService.GetCustomer(id);
            if(customer != null)
            {
                //заказчик найден, ищем контракт
                response = _contractService.GetContract(id, contractid);
                if(response != null)
                {
                    //контракт найден
                    done = true;
                }
                else
                {
                    message = $"контракт id = {contractid} заказчика customerId = {id} не найден";
                }
            }
            else
            {
                message = $"заказчик id = {id} не найден";
            }

            if(done)
            {
                return Ok(response);
            }
            else
            {
                return Ok(message);
            }
        }
       
        //получить контракты по customer id
        [HttpGet("{id}/contract/all")]
        public IActionResult GetContractAll([FromRoute] int id)
        {
            var message = "";
            var done = false;
            List<ContractResponse> contractResponse = _contractService.GetContractAll(id);
            if(contractResponse != null)
            {
                if(contractResponse.Count > 0)
                {
                    done = true;
                }
                else
                {
                    message = $"у заказчика id = {id} нет контрактов";
                }
            }
            else
            {
                message = $"заказчик id = {id} не существует";
            }

            if (done)
            {
                return Ok(contractResponse);
            }
            else
            {
                return Ok(message);
            }
        }

        //получить инвойс по task id
        [HttpGet("{id}/contract/{contractid}/task/{taskid}/invoice")]
        public IActionResult GetInvoice([FromRoute] int id, [FromRoute] int contractid, [FromRoute] int taskid)
        {
            InvoiceResponse response = _invoiseService.GetInvoice(id, contractid, taskid);
            if(response != null)
            {
                return Ok(response);
            }
            else
            {
                return Ok($"счет для задачи id = {taskid}, контракта contractId = {contractid}, заказчика customerId = {id} не найден");
            }
        }

        //получить все инвойсы по контракту
        [HttpGet("{id}/contract/{contractid}/invoice/all")]
        public IActionResult GetInvoiceAll([FromRoute] int id, [FromRoute] int contractid)
        {
            List<InvoiceResponse> response = _invoiseService.GetInvoiceAll(id, contractid);
            if(response != null)
            {
                return Ok(response);
            }
            else
            {
                return Ok($"счета контракта id = {contractid}, заказчика customerId = {id} не найдены");
            }        
        }
       
        //зарегистрировать задачу
        [HttpPost("{id}/contract/{contractid}/task")]
        public IActionResult RegisterTask([FromRoute] int id, [FromRoute] int contractid)
        {
            TaskResponse response = null;
            var taskId = _dictionaryGlobal._taskid;
            var message = "";
            var done = false;

            try
            {
                //ищем контракт
                ContractModel contract = null;
                ContractResponse contractResponse =  _contractService.GetContract(id, contractid);

                if(contractResponse != null)
                {
                    contract = _contractService.GetModel(contractResponse);
                    if (contract != null)
                    {
                        //контракт найден
                        TaskModel task = new TaskModel()
                        {
                            Id = taskId,
                            Contract = contract,
                            Employees = new List<TaskEmployeeModel>()
                        };

                        response = _taskService.RegisterTask(task);
                        if (response != null)
                        {
                            done = true;
                        }
                        else
                        {
                            message = $"не удалось добавить задачу контракту id = {contractid}, заказчик customerId = {id}";
                        }
                    }
                    else
                    {
                        message = $"не удалось получить объект контракта id = {contractid}";            
                    }
                }
                else
                {
                    //проверяем, существует ли заказчик
                    CustomerResponse customerResponse = _customerService.GetCustomer(id);
                    if(customerResponse == null)
                    {
                        //заказчик не существует
                        message = $"заказчик id = {id} не найден";
                    } 
                    else
                    {
                        message = $"контракт id = {contractid} не найден у заказчика customerId = {id}";
                    }
                }
            }
            catch (Exception ex)
            {
                message = $"не удалось добавить задачу контракту id {contractid}, заказчик customerId = {id}, ошибка {ex.Message}";
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

        //получить задачу по id
        [HttpGet("{id}/contract/{contractid}/task/{taskid}")]
        public IActionResult GetTask([FromRoute] int id, [FromRoute] int contractid, [FromRoute] int taskid)
        {
            var done = false;
            var message = "";
            TaskResponse response = null;
            response = _taskService.GetTask(id, contractid, taskid);

            if (response != null)
            {
                done = true;
            }
            else
            {
                //проверим, существует ли заказчик
                CustomerResponse customerResponse = _customerService.GetCustomer(id);
                if(customerResponse != null)
                {
                    //заказчик найден
                    //проверим, существует ли контракт
                    ContractResponse contractResponse = _contractService.GetContract(id, contractid);
                    if(contractResponse != null)
                    {
                        //контракт существует, но response = null
                        message = $"задача id = {taskid} у заказчика customerId = {id}, контракт contractId = {contractid} не найдена";
                    }
                    else
                    {
                        message = $"контракт id = {contractid} не существует у заказчика customerId = {id}";
                    }
                }
                else
                {
                    message = $"заказчик id = {id} не найден";
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

        //получить задачи по контракту
        [HttpGet("{id}/contract/{contractid}/task/all")]
        public IActionResult GetTaskAll([FromRoute] int id, [FromRoute] int contractid) 
        {
            var message = "";
            var done = false;
            List<TaskResponse> taskResponse = null;

            CustomerResponse customerResponse = _customerService.GetCustomer(id);
            if (customerResponse != null)
            {
                //заказчик существует
                taskResponse = _taskService.GetTaskAll(id, contractid);
                if (taskResponse != null)
                {
                    if (taskResponse.Count > 0)
                    {
                        done = true;
                    }
                    else
                    {
                        message = $"у заказчика id = {id}, контракт contractId = {contractid} нет задач";
                    }
                }
                else
                {
                    message = $"контракт id = {contractid} у заказчика customerId = {id} не найден";
                }
            }
            else
            {
                message = $"заказчик id = {id} не существует";
            }

            if (done)
            {
                return Ok(taskResponse);
            }
            else
            {
                return Ok(message);
            }  
        }
    }
}

        