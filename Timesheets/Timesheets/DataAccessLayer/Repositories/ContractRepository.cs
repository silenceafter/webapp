using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Responses;
using Timesheets.DataAccessLayer.Interfaces;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly DictionariesGlobal _dictionaryGlobal;
        private ICustomerRepository _customerRepository;

        public ContractRepository(DictionariesGlobal dictionariesGlobal, ICustomerRepository customerRepository)
        {
            _dictionaryGlobal = dictionariesGlobal;
            _customerRepository = customerRepository;
        }

        public bool RegisterContract(ContractModel contract)
        {
            _dictionaryGlobal.contracts.Add(contract); //сохраняем контракт
            CustomerModel customer = contract.Customer;
            //
            if (customer != null)
            {
                customer.Contracts.Add(contract); //копия у кастомера                    
            }
            
            _dictionaryGlobal._contractid++;
            return true;
        }
        
        public ContractModel GetContract(int id, int contractid)
        {
            ContractModel currentContract = null;
            CustomerModel customer = _customerRepository.GetCustomer(id);
            if (customer != null)
            {
                //заказчик найден, ищем контракт
                var contracts = customer.Contracts;                
                foreach(var contract in contracts)
                {
                    if (contract.Id.Equals(contractid))
                    {
                        //контракт найден
                        currentContract = contract;
                        break;
                    }
                }
            }
            return currentContract;
        }

        public List<ContractModel> GetContractAll(int id)
        {
            List<ContractModel> contracts = null;
            CustomerModel customer = _customerRepository.GetCustomer(id);
            if (customer != null)
            {
                //заказчик найден, берем контракты
                contracts = customer.Contracts;
            }
            return contracts;
        }
    }
}
