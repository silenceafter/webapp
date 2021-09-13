using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Responses;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Services.Interfaces
{
    public interface IInvoiceService
    {
        InvoiceResponse RegisterInvoice(InvoiceModel invoice);
        InvoiceResponse GetInvoice(int id, int contractid, int taskid);
        List<InvoiceResponse> GetInvoiceAll(int id, int contractid);
    }
}
