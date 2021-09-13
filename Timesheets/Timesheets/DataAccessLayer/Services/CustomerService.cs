using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Responses;
using Timesheets.DataAccessLayer.Interfaces;
using Timesheets.DataAccessLayer.Models;
using Timesheets.DataAccessLayer.Services.Interfaces;

namespace Timesheets.DataAccessLayer.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly DictionariesGlobal _dictionaryGlobal;
        private ICustomerRepository _customerRepository;
        public CustomerService(DictionariesGlobal dictionariesGlobal, ICustomerRepository customerRepository)
        {
            _dictionaryGlobal = dictionariesGlobal;
            _customerRepository = customerRepository;
        }

        public CustomerResponse RegisterCustomer(CustomerModel customer)
        {
            CustomerResponse response = null;
            try
            {
                if (_customerRepository.RegisterCustomer(customer))
                {
                    //сохранение прошло
                    response = new CustomerResponse()
                    {
                        Id = customer.Id,
                        BankAccount = customer.BankAccount,
                        Contracts = new List<int>()
                    };
                }
            }
            catch (Exception ex)
            {
                //
            }
            return response;
        }

        public CustomerResponse GetCustomer(int id)
        {
            CustomerResponse response = null;
            CustomerModel customer = _customerRepository.GetCustomer(id);
            if (customer != null)
            {
                //заказчик нашелся
                //для response оставим только id, не ссылки на объекты
                List<int> responseContracts = new List<int>();
                foreach (var contract in customer.Contracts)
                {
                    responseContracts.Add(contract.Id);
                }

                response = new CustomerResponse()
                {
                    Id = customer.Id,
                    BankAccount = customer.BankAccount,
                    Contracts = responseContracts
                };
            }
            return response;
        }

        public List<CustomerResponse> GetCustomerAll()
        {
            List<CustomerResponse> customerResponse = null;
            List<CustomerModel> customers = _customerRepository.GetCustomerAll();

            if(customers != null)
            {
                customerResponse = new List<CustomerResponse>();
                foreach (var customer in customers)
                {
                    var contracts = customer.Contracts;
                    List<int> contractResponse = new List<int>();
                    foreach (var contract in contracts)
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
            }
            return customerResponse;
        }

        public CustomerModel GetModel(CustomerResponse customerResponse)
        {
            if (customerResponse != null)
            {
                CustomerModel customer = _customerRepository.GetCustomer(customerResponse.Id);
                if (customer != null)
                {
                    //объект найден
                    return customer;
                }
            }   
            return null;
        }
    }
}
