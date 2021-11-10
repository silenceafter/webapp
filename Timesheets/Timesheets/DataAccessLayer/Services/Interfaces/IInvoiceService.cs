using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Requests;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Services.Interfaces
{
    public interface IInvoiceService
    {
        InvoiceModel RegisterInvoice(InvoiceRequest invoice);
        InvoiceModel GetInvoice(int id, int contractid, int taskid);
        bool SetInvoice(int customerid, InvoiceModel invoice);
        List<InvoiceModel> GetInvoiceAll(int contractid);
    }
}
