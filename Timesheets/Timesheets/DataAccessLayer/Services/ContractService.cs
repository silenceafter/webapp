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
    public class ContractService : IContractService
    {
        private readonly DictionariesGlobal _dictionaryGlobal;
        private IContractRepository _contractRepository;
        public ContractService(DictionariesGlobal dictionariesGlobal, IContractRepository contractRepository)
        {
            _dictionaryGlobal = dictionariesGlobal;
            _contractRepository = contractRepository;
        }

        public ContractResponse RegisterContract(ContractModel contract)
        {
            ContractResponse response = null;
            try
            {
                if (_contractRepository.RegisterContract(contract))
                {
                    //сохранение прошло
                    response = new ContractResponse()
                    {                        
                        Id = contract.Id,
                        Customer = contract.Customer.Id,
                        Tasks = new List<int>(),
                        Invoices = new List<int>()
                    };
                }
            }
            catch (Exception ex)
            {
                //
            }
            return response;
        }
        
        public ContractResponse GetContract(int id, int contractid)
        {
            ContractResponse response = null;
            ContractModel contract = _contractRepository.GetContract(id, contractid);
            if (contract != null)
            {
                //контракт нашелся
                //для response оставим только id, не ссылки на объекты
                List<int> responseTasks = new List<int>();                
                foreach (var task in contract.Tasks)
                {
                    responseTasks.Add(task.Id);
                }
                
                List<int> responseInvoices = new List<int>();
                foreach (var invoice in contract.Invoices)
                {
                    responseInvoices.Add(invoice.Id);
                }

                response = new ContractResponse()
                {
                    Id = contract.Id,
                    Customer = contract.Customer.Id,
                    Tasks = responseTasks,
                    Invoices = responseInvoices
                };
            }
            return response;
        }
        
        public List<ContractResponse> GetContractAll(int id)
        {
            List<ContractResponse> contractResponse = null;
            List<ContractModel> contracts = _contractRepository.GetContractAll(id);

            if (contracts != null)
            {
                contractResponse = new List<ContractResponse>();
                foreach (var contract in contracts)
                {
                    List<int> taskResponse = new List<int>();
                    foreach(var task in contract.Tasks)
                    {
                        taskResponse.Add(task.Id);
                    }

                    List<int> invoiceResponse = new List<int>();
                    foreach(var invoice in contract.Invoices)
                    {
                        invoiceResponse.Add(invoice.Id);
                    }

                    contractResponse.Add(new ContractResponse()
                    {
                        Id = contract.Id,
                        Customer = contract.Customer.Id,
                        Tasks = taskResponse,
                        Invoices = invoiceResponse
                    });
                }
            }
            return contractResponse;
        }
        public ContractModel GetModel(ContractResponse contractResponse)
        {
            if (contractResponse != null)
            {
                ContractModel contract = _contractRepository.GetContract(contractResponse.Customer, contractResponse.Id);
                if (contract != null)
                {
                    //объект найден
                    return contract;
                }
            }
            return null;
        }
    }
}
