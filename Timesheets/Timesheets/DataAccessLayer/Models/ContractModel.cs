using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.DataAccessLayer.Models
{
    public class ContractModel
    {
        public int Id { get; set; }
        public CustomerModel Customer { get; set; }
        public List<TaskModel> Tasks { get; set; }
        public List<InvoiceModel> Invoices { get; set; } //можно посчитать стоимость контракта
    }
}
