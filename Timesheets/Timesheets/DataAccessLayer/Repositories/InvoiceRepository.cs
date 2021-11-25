using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Timesheets.Data;
using Timesheets.DataAccessLayer.Interfaces;
using Timesheets.DataAccessLayer.Models;
using Timesheets.DataAccessLayer.Repositories.Interfaces;

namespace Timesheets.DataAccessLayer.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private TimesheetContext _context;
        private readonly ILogger<InvoiceRepository> _logger;

        public InvoiceRepository(
            TimesheetContext context,
            ILogger<InvoiceRepository> logger
            )
        {
            _context = context;
            _logger = logger;
        }

        public int RegisterInvoice(InvoiceDto invoice)
        {
            _logger.LogInformation("RegisterInvoice() запуск метода");
            if (invoice != null)
            {
                try
                {
                    _context.Invoices.Add(invoice);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"RegisterInvoice() ошибка, {ex.Message}");
                }
                return invoice.Id;
            }
            return 0;
        }

        public InvoiceDto GetInvoice(int id, int contractid, int taskid)
        {
            _logger.LogInformation("GetInvoice() запуск метода");
            return _context.Invoices
                .Where(row => row.ContractId == contractid)
                .Where(row => row.TaskId == taskid)
                .SingleOrDefault();
        }

        public bool SetInvoice(int customerid, InvoiceDto invoice)
        {
            _logger.LogInformation("SetInvoice() запуск метода");
            if (invoice != null)
            {
                try
                {
                    var response = this.GetInvoice(customerid, invoice.ContractId, invoice.TaskId);
                    if (response != null)
                    {
                        response.Cost = invoice.Cost;
                        response.PayDone = invoice.PayDone;
                    }
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"SetInvoice() ошибка, {ex.Message}");
                }
                return true;
            }
            return false;
        }

        public List<InvoiceDto> GetInvoiceAll(int contractid)
        {
            _logger.LogInformation("GetInvoiceAll() запуск метода");
            return _context.Invoices
                .Where(row => row.ContractId == contractid)
                .ToList();
        }
    }
}
