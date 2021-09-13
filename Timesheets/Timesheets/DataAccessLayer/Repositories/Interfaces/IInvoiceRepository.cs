using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.DataAccessLayer.Interfaces;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Repositories.Interfaces
{
    public interface IInvoiceRepository : IRepository<InvoiceModel>
    {
        bool RegisterInvoice(InvoiceModel invoice);
        InvoiceModel GetInvoice(int id, int contractid, int taskid);
        List<InvoiceModel> GetInvoiceAll(int id, int contractid);
    }
}
