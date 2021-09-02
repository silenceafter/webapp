using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Responses;

namespace Timesheets.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly DictionariesGlobal _dictionaryGlobal;
        public CustomerController(DictionariesGlobal dictionariesGlobal)
        {
            _dictionaryGlobal = dictionariesGlobal;           
        }

        //регистрация, действие по умолчанию
        [HttpPost]
        public IActionResult RegisterCustomer()
        {
            var customers = _dictionaryGlobal.customers;
            var customerId = _dictionaryGlobal._customerid;
            CustomerResponse response = null;            
            var message = "";
            var done = false;

            try
            {
                var customer = new CustomerDto()
                {
                    Id = customerId,
                    Contracts = new List<ContractDto>(),
                    BankAccount = 100000.0
                };
                customers.Add(customer);
                response = new CustomerResponse()
                {
                    Id = customer.Id,
                    BankAccount = customer.BankAccount,
                    Contracts = new List<int>()
                };
                done = true;
            }
            catch(Exception ex)
            {
                message = $"добавить заказчика не удалось, ошибка {ex.Message}";
            }
            
            if (done)
            {
                _dictionaryGlobal._customerid++;
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
            var customers = _dictionaryGlobal.customers;
            CustomerResponse response = null;
            var find = false;
            foreach(var customer in customers)
            {
                if(customer.Id.Equals(id))
                {
                    //кастомер найден
                    find = true;
                    //для response оставим только id, не ссылки на объекты
                    List<int> responseContracts = new List<int>();
                    foreach(var contract in customer.Contracts)
                    {
                        responseContracts.Add(contract.Id);
                    }

                    response = new CustomerResponse()
                    {
                        Id = customer.Id,
                        BankAccount = customer.BankAccount,
                        Contracts = responseContracts
                    };
                    break;
                }
            }

            if (find)
            {              
                return Ok(response);
            } else
            {
                return Ok($"заказчик id = {id} не найден");
            }          
        }

        //получить всех закачиков
        [HttpGet("all")]
        public IActionResult GetCustomerAll()
        {
            var customers = _dictionaryGlobal.customers;
            List<CustomerResponse> customerResponse = new List<CustomerResponse>();

            foreach (var customer in customers)
            {
                var contracts = customer.Contracts;
                List<int> contractResponse = new List<int>();
                foreach(var contract in contracts)
                {
                    contractResponse.Add(contract.Id);
                }

                customerResponse.Add(new CustomerResponse()
                {
                    Id = customer.Id,
                    BankAccount = customer.BankAccount,
                    Contracts = contractResponse
                });
            }
            return Ok(customerResponse);
        }

        //зарегистрировать контракт по customer id
        [HttpPost("{id}/contract")]
        public IActionResult RegisterContract([FromRoute] int id)
        {
            var customers = _dictionaryGlobal.customers;
            var contractId = _dictionaryGlobal._contractid;
            ContractResponse response = null;
            //ContractDto currentContract = null;
            var message = $"заказчик id = {id} не найден";
            var find = false;

            foreach (var customer in customers)
            {
                if (customer.Id.Equals(id))
                {
                    //нашли нужного кастомера
                    find = true;
                    var done = false;
                    ContractDto contract = null;

                    try
                    {
                        contract = new ContractDto()
                        {
                            Id = contractId,
                            Customer = customer,
                            Tasks = new List<TaskDto>(),
                            Invoices = new List<InvoiceDto>()
                        };
                        done = true;
                    } catch(Exception ex)
                    {
                        find = false;
                        message = $"не удалось добавить контракт у заказчика id = {id}, ошибка {ex.Message}";
                    }

                    if (done)
                    {
                        _dictionaryGlobal.contracts.Add(contract); //сохраняем контракт
                        customer.Contracts.Add(contract); //копия у кастомера                    
                        _dictionaryGlobal._contractid++;
                        //
                        response = new ContractResponse()
                        {
                            Id = contract.Id,
                            Customer = contract.Customer.Id,
                            Tasks = new List<int>(),
                            Invoices = new List<int>()
                        };
                    }                  
                    break;
                }
            }

            if (find)
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
            var customers = _dictionaryGlobal.customers;                       
            var find = false;
            var message = $"заказчик  customerId = {id} не найден";
            ContractResponse response = null; 

            foreach(var customer in customers)
            {
                if (customer.Id.Equals(id))
                {
                    //нашли кастомера
                    find = true;
                    var contracts = customer.Contracts;
                    //
                    find = false;
                    foreach (var contract in contracts)
                    if(contract.Id.Equals(contractid))
                    {
                        find = true;
                        List<int> tasks = new List<int>();
                        List<int> invoices = new List<int>();
                        //
                        foreach(var task in contract.Tasks)
                        {
                            tasks.Add(task.Id);
                        }

                        foreach(var invoice in contract.Invoices)
                        {
                            invoices.Add(invoice.Id);
                        }

                        response = new ContractResponse()
                        {
                            Id = contract.Id,
                            Customer = contract.Customer.Id,
                            Tasks = tasks,
                            Invoices = invoices
                        };                        
                        break;
                    }
                    if (!find)
                    {
                        message = $"контракт contractId = {contractid} у заказчика customerId = {id} не найден";
                    }
                    break;
                }
            }

            if (find)
            {
                return Ok(response);
            } else
            {
                return Ok(message);
            }            
        }

        //получить контракты по customer id
        [HttpGet("{id}/contract/all")]
        public IActionResult GetContractAll([FromRoute] int id)
        {
            var customers = _dictionaryGlobal.customers;
            List<ContractDto> contracts = null;          
            List<ContractResponse> response = null;
            var find = false;
            var done = false;
            
            foreach(var customer in customers)
            {
                if (customer.Id.Equals(id))
                {
                    find = true;
                    contracts = customer.Contracts;                    
                }
            }

            if (find)
            {
                response = new List<ContractResponse>();
                foreach(var contract in contracts)
                {
                    List<int> tasks = new List<int>();
                    List<int> invoices = new List<int>();
                    //получаем id тасков
                    foreach(var task in contract.Tasks)
                    {
                        tasks.Add(task.Id);
                    }

                    //получаем id инвойсов
                    foreach(var invoice in contract.Invoices)
                    {
                        invoices.Add(invoice.Id);
                    }

                    response.Add(new ContractResponse()
                    {
                        Id = contract.Id,
                        Customer = contract.Customer.Id,
                        Tasks = tasks,
                        Invoices = invoices
                    });
                }
                return Ok(response);
            } else
            {
                return Ok($"заказчик customerId = {id} не найден");
            } 
        }

        //получить инвойс по task id
        [HttpGet("{id}/contract/{contractid}/task/{taskid}/invoice")]
        public IActionResult GetInvoice([FromRoute] int id, [FromRoute] int contractid, [FromRoute] int taskid)
        {
            var customers = _dictionaryGlobal.customers;
            CustomerDto currentCustomer = null;
            ContractDto currentContract = null;
            InvoiceResponse response = null;
            var find = false;
            var done = false;
            var message = "";

            //ищем заказчика
            foreach(var customer in customers)
            {
                if (customer.Id.Equals(id))
                {
                    //заказчик найден
                    find = true;
                    currentCustomer = customer;
                    break;
                }
            }

            if(find)
            {
                //ищем контракт
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
                    //ищем в инвойсах ссылку на задание
                    find = false;
                    var invoices = currentContract.Invoices;
                    foreach(var invoice in invoices)
                    {
                        if (invoice.Task.Id.Equals(taskid))
                        {
                            //задание найдено
                            find = true;
                            response = new InvoiceResponse()
                            {
                                Id = invoice.Id,
                                Contract = invoice.Contract.Id,
                                Cost = invoice.Cost,
                                PayDone = invoice.PayDone,
                                Task = invoice.Task.Id
                            };
                            done = true;
                            break;
                        }
                    }

                    if (!find)
                    {
                        //ищем задание, т.к. задание без инвойса = невыполненное задание, но оно есть в списке
                        find = false;
                        var tasks = currentContract.Tasks;
                        foreach(var task in tasks)
                        {
                            if (task.Id.Equals(taskid))
                            {
                                //задача найдена
                                find = true;
                                message = $"задание id = {taskid} найдено, но счет не сформирован, задание не выполнено";
                                break;
                            }
                        }

                        if (!find)
                        {
                            message = $"задание id = {taskid} не найдено у контракта contractId = {contractid}, заказчика customerId = {id}";
                        }
                    }
                }
                else
                {
                    message = $"контракт id = {contractid} не найден у заказчика customerId = {id}";
                }
            }
            else
            {
                message = $"заказчик id = {id} не найден";
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

        //получить все инвойсы по контракту
        [HttpGet("{id}/contract/{contractid}/invoice/all")]
        public IActionResult GetInvoiceAll([FromRoute] int id, [FromRoute] int contractid)
        {
            var customers = _dictionaryGlobal.customers;            
            CustomerDto currentCustomer = null;
            ContractDto currentContract = null;
            List<InvoiceResponse> response = null;
            var find = false;
            var done = false;
            var message = "";
            
            foreach(var customer in customers)
            {
                if (customer.Id.Equals(id))
                {
                    //нашли кастомера
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
                    //смотрим инвойсы по контракту
                    List<InvoiceResponse> invoices = new List<InvoiceResponse>();
                    foreach(var invoice in currentContract.Invoices)
                    {
                        invoices.Add(new InvoiceResponse()
                        {
                            Id = invoice.Id,
                            Contract = invoice.Contract.Id,
                            Task = invoice.Task.Id,
                            Cost = invoice.Cost,
                            PayDone = invoice.PayDone                            
                        });
                    }

                    response = invoices;
                    done = true;
                }
                else
                {
                    message = $"контракт id = {contractid} не найден у закачика customerId = {id}";
                }
            }
            else
            {
                message = $"заказчик id = {id} не найден";
            }
            
            if (done)
            {
                return Ok(response);
            } else
            {
                return Ok(message);
            }
        }

        //зарегистрировать задачу
        [HttpPost("{id}/contract/{contractid}/task")]
        public IActionResult RegisterTask([FromRoute] int id, [FromRoute] int contractid)
        {
            var customers = _dictionaryGlobal.customers;
            var taskId = _dictionaryGlobal._taskid;
            var message = $"заказчик id = {id} не найден";
            TaskResponse response = null;
            var find = false;
            var done = false;

            foreach (var customer in customers)
            {
                if (customer.Id.Equals(id))
                {
                    //нашли кастомера
                    find = true;
                    //
                    find = false;
                    var contracts = customer.Contracts;
                    foreach(var contract in contracts)
                    {
                        if (contract.Id.Equals(contractid))
                        {
                            //нашли контракт
                            find = true;
                            var tasks = contract.Tasks;
                            TaskDto task = null;
                            
                            try
                            {
                                task = new TaskDto()
                                {
                                    Id = taskId,
                                    Contract = contract,
                                    Employees = new List<TaskEmployeeDto>()
                                };                                                        
                                _dictionaryGlobal.tasks.Add(task);                                
                                contract.Tasks.Add(task);
                                done = true;
                            }
                            catch(Exception ex)
                            {
                                message = $"задача {taskId} не добавлена контракту {contractid} заказчика {id}, ошибка {ex.Message}";
                            }
                            
                            if (done)
                            {
                                _dictionaryGlobal._taskid++;
                                response = new TaskResponse()
                                {
                                    Id = task.Id,
                                    Contract = task.Contract.Id,
                                    Employees = new List<int>()
                                };
                            }
                            break;
                        }
                    }

                    if (!find)
                    {
                        message = $"контракт contractId = {contractid} заказчика id = {id} не найден";
                        break;
                    }
                }
            }

            if (find)
            {
                return Ok(response);
            } else
            {
                return Ok(message);
            }           
        }

        //получить задачу по id
        [HttpGet("{id}/contract/{contractid}/task/{taskid}")]
        public IActionResult GetTask([FromRoute] int id, [FromRoute] int contractid, [FromRoute] int taskid)
        {
            var customers = _dictionaryGlobal.customers;
            CustomerDto currentCustomer = null;
            ContractDto currentContract = null;
            TaskResponse response = null;
            var find = false;
            var done = false;
            var message = "";

            //ищем заказчика
            foreach(var customer in customers)
            {
                if (customer.Id.Equals(id))
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
                        //найден контракт
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
                            //задача найдена
                            find = true;
                            List<int> taskEmployees = new List<int>();
                            foreach(var taskEmployee in task.Employees)
                            {
                                taskEmployees.Add(taskEmployee.Id);
                            }

                            response = new TaskResponse()
                            {
                                Id = task.Id,
                                Contract = task.Contract.Id,
                                Employees = taskEmployees
                            };
                            done = true;
                            break;
                        }
                    }

                    if(!find)
                    {
                        message = $"задача id = {taskid} не найдена у контракта contractId = {contractid}, заказчик customerId = {id}";
                    }
                }
                else 
                {
                    message = $"контракт id = {contractid} не найден у заказчика customerId = {id}";
                }
            }
            else
            {
                message = $"заказчик id = {id} не найден";
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
            var customers = _dictionaryGlobal.customers;
            CustomerDto currentCustomer = null;
            ContractDto currentContract = null;
            List<TaskResponse> response = new List<TaskResponse>();
            var find = false;
            var done = false;
            var message = "";

            //ищем заказчика
            foreach (var customer in customers)
            {
                if (customer.Id.Equals(id))
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
                foreach (var contract in contracts)
                {
                    if (contract.Id.Equals(contractid))
                    {
                        //найден контракт
                        find = true;
                        currentContract = contract;
                        break;
                    }
                }

                if (find)
                {
                    try
                    {
                        var tasks = currentContract.Tasks;
                        TaskResponse taskResponse = null;
                        foreach (var task in tasks)
                        {
                            List<int> taskEmployees = new List<int>();
                            foreach (var taskEmployee in task.Employees)
                            {
                                taskEmployees.Add(taskEmployee.Id);
                            }

                            taskResponse = new TaskResponse()
                            {
                                Id = task.Id,
                                Contract = task.Contract.Id,
                                Employees = taskEmployees
                            };
                            response.Add(taskResponse);
                        }
                    }
                    catch(Exception ex)
                    {
                        message = $"При формировании ответа произошла ошибка {ex.Message}";
                    }
                    done = true;
                }
                else
                {
                    message = $"контракт id = {contractid} не найден у заказчика customerId = {id}";
                }
            }
            else
            {
                message = $"заказчик id = {id} не найден";
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
