using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Responses;
using Timesheets.DataAccessLayer.Models;
using Timesheets.DataAccessLayer.Repositories.Interfaces;
using Timesheets.DataAccessLayer.Services.Interfaces;

namespace Timesheets.DataAccessLayer.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly DictionariesGlobal _dictionaryGlobal;
        private IInvoiceRepository _invoiceRepository;

        public InvoiceService(DictionariesGlobal dictionariesGlobal, IInvoiceRepository invoiceRepository)
        {
            _dictionaryGlobal = dictionariesGlobal;
            _invoiceRepository = invoiceRepository;
        }

        public InvoiceResponse RegisterInvoice(InvoiceModel invoice)
        {
            if(_invoiceRepository.RegisterInvoice(invoice))
            {
                //счет зарегистрирован
                InvoiceResponse response = new InvoiceResponse()
                {
                    Id = invoice.Id,
                    Contract = invoice.Contract.Id,
                    Cost = invoice.Cost,
                    PayDone = invoice.PayDone,
                    Task = invoice.Task.Id
                };
                return response;
            }
            return null;
        }
        public InvoiceResponse GetInvoice(int id, int contractid, int taskid)
        {
            InvoiceResponse response = null;
            InvoiceModel invoice = _invoiceRepository.GetInvoice(id, contractid, taskid);
            if(invoice != null)
            {
                response = new InvoiceResponse()
                {
                    Id = invoice.Id,
                    Contract = invoice.Contract.Id,
                    Cost = invoice.Cost,
                    PayDone = invoice.PayDone,
                    Task = invoice.Task.Id
                };
            }
            return response;
        }

        public List<InvoiceResponse> GetInvoiceAll(int id, int contractid)
        {
            List<InvoiceResponse> response = null;
            List<InvoiceModel> invoices = _invoiceRepository.GetInvoiceAll(id, contractid);
            if(invoices != null)
            {
                response = new List<InvoiceResponse>();
                foreach(var invoice in invoices)
                {
                    response.Add(new InvoiceResponse()
                    {
                        Id = invoice.Id,
                        Contract = invoice.Contract.Id, 
                        Cost = invoice.Cost,
                        PayDone = invoice.PayDone,
                        Task = invoice.Task.Id
                    });
                }
            }
            return response;
        }
    }
}
