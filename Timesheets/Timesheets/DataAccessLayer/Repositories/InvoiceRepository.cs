using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.DataAccessLayer.Interfaces;
using Timesheets.DataAccessLayer.Models;
using Timesheets.DataAccessLayer.Repositories.Interfaces;

namespace Timesheets.DataAccessLayer.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly DictionariesGlobal _dictionaryGlobal;
        private IContractRepository _contractRepository;
        private ITaskRepository _taskRepository;

        public InvoiceRepository(
            DictionariesGlobal dictionariesGlobal, 
            IContractRepository contractRepository,
            ITaskRepository taskRepository
            )
        {
            _dictionaryGlobal = dictionariesGlobal;
            _contractRepository = contractRepository;
            _taskRepository = taskRepository;
        }

        public bool RegisterInvoice(InvoiceModel invoice)
        {
            var done = false;
            try
            {
                _dictionaryGlobal.invoices.Add(invoice);
                ContractModel contract = invoice.Task.Contract;
                if (contract != null)
                {
                    contract.Invoices.Add(invoice);
                }
                _dictionaryGlobal._invoiceid++;
                done = true;
            }
            catch
            {
            }
            return done;
        }

        public InvoiceModel GetInvoice(int id, int contractid, int taskid)
        {
            TaskModel task = _taskRepository.GetTask(id, contractid, taskid);
            if(task != null)
            {
                //задача найдена
                List<InvoiceModel> invoices = GetInvoiceAll(id, contractid);
                foreach(var invoice in invoices)
                {
                    if (invoice.Id.Equals(taskid))
                    {
                        //счет найден
                        return invoice;                            
                    }
                }
            }
            return null;
        }

        public List<InvoiceModel> GetInvoiceAll(int id, int contractid)
        {
            ContractModel contract = _contractRepository.GetContract(id, contractid);
            if(contract != null)
            {
                //контракт найден, возвращаем счета
                return contract.Invoices;
            }
            return null;
        }
    }
}
