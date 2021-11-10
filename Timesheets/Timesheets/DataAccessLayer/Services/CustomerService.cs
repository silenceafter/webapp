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
    public class CustomerService : ICustomerService
    {
        private TimesheetContext _context;
        private readonly ILogger<CustomerService> _logger;
        private IContractService _contractService;
        private ICustomerRepository _customerRepository;   

        public CustomerService(
            TimesheetContext context,
            ILogger<CustomerService> logger,
            IContractService contractService,
            ICustomerRepository customerRepository           
            )
        {
            _context = context;
            _logger = logger;
            _contractService = contractService;
            _customerRepository = customerRepository;
        }

        public CustomerModel RegisterCustomer(CustomerRequest customer)
        {
            _logger.LogInformation("RegisterCustomer() запуск метода");
            var newId = _customerRepository.RegisterCustomer(new CustomerDto()
            {
                BankAccount = customer.BankAccount
            });
            if (newId > 0)
            {
                _logger.LogInformation("RegisterCustomer() завершено");
                return new CustomerModel() 
                {
                    Id = newId,
                    BankAccount = customer.BankAccount,
                    Contracts = new List<int>()
                };
            } else
            {
                return null;
            }            
        }

        public CustomerModel GetCustomer(int id)
        {
            _logger.LogInformation("GetCustomer() запуск метода");
            var customer = _customerRepository.GetCustomer(id);
            if (customer != null)
            {
                //контракты
                var contractModels = _contractService.GetContractAll(id);
                List<int> contracts = new List<int>();
                if (contractModels != null)
                {
                    foreach (var contractModel in contractModels)
                    {
                        contracts.Add(contractModel.Id);
                    };
                }

                _logger.LogInformation("GetCustomer() завершено");
                return new CustomerModel()
                {
                    Id = customer.Id,
                    BankAccount = customer.BankAccount,
                    Contracts = contracts
                };                
            }
            return null;
        }

        public bool SetCustomer(CustomerModel customer)
        {
            _logger.LogInformation("SetCustomer() запуск метода");
            return _customerRepository.SetCustomer(new CustomerDto()
            {
                Id = customer.Id,
                BankAccount = customer.BankAccount
            });
        }

        public List<CustomerModel> GetCustomerAll()
        {
            _logger.LogInformation("GetCustomerAll() запуск метода");
            var customersDto = _customerRepository.GetCustomerAll();
            List<CustomerModel> customers = new List<CustomerModel>();
            if (customersDto != null)
            {
                foreach(var customerDto in customersDto)
                {
                    //контракты
                    var contractModels = _contractService.GetContractAll(customerDto.Id);
                    List<int> contracts = new List<int>();
                    if (contractModels != null)
                    {
                        foreach (var contractModel in contractModels)
                        {
                            contracts.Add(contractModel.Id);
                        };
                    }
                    
                    customers.Add(new CustomerModel()
                    {
                        Id = customerDto.Id,
                        BankAccount = customerDto.BankAccount,
                        Contracts = contracts
                    });
                }
            }
            _logger.LogInformation("GetCustomerAll() завершено");
            return customers;
        }
    }
}
