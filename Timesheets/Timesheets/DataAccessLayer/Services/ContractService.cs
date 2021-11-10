using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Timesheets.Controllers.Requests;
using Timesheets.Data;
using Timesheets.DataAccessLayer.Interfaces;
using Timesheets.DataAccessLayer.Models;
using Timesheets.DataAccessLayer.Services.Interfaces;

namespace Timesheets.DataAccessLayer.Services
{
    public class ContractService : IContractService
    {
        private TimesheetContext _context;
        private readonly ILogger<ContractService> _logger;
        private IContractRepository _contractRepository;
        private ITaskService _taskService;
        private IInvoiceService _invoiceService;

        public ContractService(
            TimesheetContext context,
            ILogger<ContractService> logger,
            IContractRepository contractRepository, 
            ITaskService taskService,
            IInvoiceService invoiceService
            )
        {
            _context = context;
            _logger = logger;
            _contractRepository = contractRepository;
            _taskService = taskService;
            _invoiceService = invoiceService;
        }

        public ContractModel RegisterContract(ContractRequest contract)
        {
            _logger.LogInformation("RegisterContract() запуск метода");
            var newId = _contractRepository.RegisterContract(new ContractDto()
            {
                CustomerId = contract.Customer 
            });

            if (newId > 0)
            {
                _logger.LogInformation("RegisterContract() завершено");
                return new ContractModel()
                {
                    Id = newId,
                    CustomerId = contract.Customer,
                    Invoices = new List<int>(),
                    Tasks = new List<int>()
                };
            }
            else
            {
                return null;
            }
        }
        
        public ContractModel GetContract(int id, int contractid)
        {
            _logger.LogInformation("GetContract() запуск метода");
            var contract = _contractRepository.GetContract(id, contractid);
            if (contract != null)
            {
                //задачи
                var tasksModels = _taskService.GetTaskAll(contractid);
                List<int> tasks = new List<int>();
                if (tasksModels != null)
                {
                    foreach (var taskModel in tasksModels)
                    {
                        tasks.Add(taskModel.Id);
                    }
                }
                
                //счета
                var invoicesModels = _invoiceService.GetInvoiceAll(contractid);
                List<int> invoices = new List<int>();
                if (invoicesModels != null)
                {
                    foreach (var invoiceModel in invoicesModels)
                    {
                        invoices.Add(invoiceModel.Id);
                    }
                }

                _logger.LogInformation("GetContract() завершено");
                return new ContractModel()
                {
                    Id = contract.Id,
                    CustomerId = contract.CustomerId,
                    Invoices = invoices,
                    Tasks = tasks
                };
            }
            return null;
        }

        public ContractModel GetContract(int contractid)
        {
            _logger.LogInformation("GetContract() запуск метода");
            var contract = _contractRepository.GetContract(contractid);
            if (contract != null)
            {
                //задачи
                var tasksModels = _taskService.GetTaskAll(contractid);
                List<int> tasks = new List<int>();
                if (tasksModels != null)
                {
                    foreach (var taskModel in tasksModels)
                    {
                        tasks.Add(taskModel.Id);
                    }
                }

                //счета
                var invoicesModels = _invoiceService.GetInvoiceAll(contractid);
                List<int> invoices = new List<int>();
                if (invoicesModels != null)
                {
                    foreach (var invoiceModel in invoicesModels)
                    {
                        invoices.Add(invoiceModel.Id);
                    }
                }

                _logger.LogInformation("GetContract() завершено");
                return new ContractModel()
                {
                    Id = contract.Id,
                    CustomerId = contract.CustomerId,
                    Invoices = invoices,
                    Tasks = tasks
                };
            }
            return null;
        }
        
        public List<ContractModel> GetContractAll(int id)
        {
            _logger.LogInformation("GetContractAll() запуск метода");
            List<ContractModel> contracts = new List<ContractModel>();
            var contractsDto = _contractRepository.GetContractAll(id);
            if (contractsDto != null)
            {
                foreach (var contractDto in contractsDto)
                {
                    //задачи
                    var tasksModels = _taskService.GetTaskAll(contractDto.Id);
                    List<int> tasks = new List<int>();
                    if (tasksModels != null)
                    {
                        foreach (var taskModel in tasksModels)
                        {
                            tasks.Add(taskModel.Id);
                        }
                    }
                    
                    //счета
                    var invoicesModels = _invoiceService.GetInvoiceAll(contractDto.Id);
                    List<int> invoices = new List<int>();
                    if (invoicesModels != null)
                    {
                        foreach (var invoiceModel in invoicesModels)
                        {
                            invoices.Add(invoiceModel.Id);
                        }
                    }
                    
                    contracts.Add(new ContractModel()
                    {
                        Id = contractDto.Id,
                        CustomerId = contractDto.CustomerId,
                        Invoices = invoices,
                        Tasks = tasks
                    });
                }
            }
            _logger.LogInformation("GetContractAll() завершено");
            return contracts;
        }
    }
}