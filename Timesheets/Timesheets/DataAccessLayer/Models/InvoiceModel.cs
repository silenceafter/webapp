using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.DataAccessLayer.Models
{
    public class InvoiceModel
    {
        public int Id { get; set; }
        public ContractModel Contract { get; set; }
        public TaskModel Task { get; set; }
        public double Cost { get; set; }
        public bool PayDone { get; set; } //статус платежа
    }
}
