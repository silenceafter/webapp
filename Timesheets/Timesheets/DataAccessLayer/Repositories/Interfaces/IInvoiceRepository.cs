using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.DataAccessLayer.Interfaces;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Repositories.Interfaces
{
    public interface IInvoiceRepository
    {
        int RegisterInvoice(InvoiceDto invoice);
        InvoiceDto GetInvoice(int id, int contractid, int taskid);
        bool SetInvoice(int customerid, InvoiceDto invoice);
        List<InvoiceDto> GetInvoiceAll(int contractid);
    }
}
