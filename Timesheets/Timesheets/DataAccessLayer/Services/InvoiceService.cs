using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Timesheets.Controllers.Requests;
using Timesheets.Data;
using Timesheets.DataAccessLayer.Models;
using Timesheets.DataAccessLayer.Repositories.Interfaces;
using Timesheets.DataAccessLayer.Services.Interfaces;

namespace Timesheets.DataAccessLayer.Services
{
    public class InvoiceService : IInvoiceService
    {
        private TimesheetContext _context;
        private readonly ILogger<InvoiceService> _logger;
        private IInvoiceRepository _invoiceRepository;

        public InvoiceService(
            TimesheetContext context,
            ILogger<InvoiceService> logger,
            IInvoiceRepository invoiceRepository
            )
        {
            _context = context;
            _logger = logger;
            _invoiceRepository = invoiceRepository;
        }

        public InvoiceModel RegisterInvoice(InvoiceRequest invoice)
        {
            _logger.LogInformation("RegisterInvoice() запуск метода");
            var newId = _invoiceRepository.RegisterInvoice(new InvoiceDto()
            {
                ContractId = invoice.Contract,
                TaskId = invoice.Task,
                Cost = invoice.Cost               
            });

            if (newId > 0)
            {
                _logger.LogInformation("RegisterInvoice() завершено");
                return new InvoiceModel()
                {
                    Id = newId,
                    ContractId = invoice.Contract,
                    TaskId = invoice.Task,
                    Cost = invoice.Cost,
                    PayDone = false                    
                };
            }
            else
            {
                return null;
            }
        }
        public InvoiceModel GetInvoice(int id, int contractid, int taskid)
        {
            _logger.LogInformation("GetInvoice() запуск метода");
            var invoice = _invoiceRepository.GetInvoice(id, contractid, taskid);
            if (invoice != null)
            {
                _logger.LogInformation("GetInvoice() завершено");
                return new InvoiceModel()
                {
                    Id = invoice.Id,
                    ContractId = invoice.ContractId,
                    TaskId = invoice.TaskId,
                    Cost = invoice.Cost,
                    PayDone = invoice.PayDone
                };
            }
            return null;
        }

        public bool SetInvoice(int customerid, InvoiceModel invoice)
        {
            _logger.LogInformation("SetInvoice() запуск метода");
            if (invoice != null)
            {
                _logger.LogInformation("SetInvoice() завершено");
                return _invoiceRepository.SetInvoice(customerid, new InvoiceDto()
                {
                    Id = invoice.Id,
                    ContractId = invoice.ContractId,
                    TaskId = invoice.TaskId,
                    Cost = invoice.Cost,
                    PayDone = invoice.PayDone
                });
            }
            return false;
        }

        public List<InvoiceModel> GetInvoiceAll(int contractid)
        {
            _logger.LogInformation("GetInvoiceAll() запуск метода");
            var invoicesDto = _invoiceRepository.GetInvoiceAll(contractid);
            List<InvoiceModel> invoices = new List<InvoiceModel>();
            if (invoicesDto != null)
            {
                foreach (var invoiceDto in invoicesDto)
                {
                    invoices.Add(new InvoiceModel()
                    {
                        Id = invoiceDto.Id,
                        ContractId = invoiceDto.ContractId,
                        TaskId = invoiceDto.TaskId,
                        Cost = invoiceDto.Cost,
                        PayDone = invoiceDto.PayDone
                    });
                }
            }
            _logger.LogInformation("GetInvoiceAll() завершено");
            return invoices;
        }       
    }
}
