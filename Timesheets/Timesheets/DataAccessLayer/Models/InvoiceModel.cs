using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.DataAccessLayer.Models
{
    public class InvoiceModel
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public int TaskId { get; set; }
        public double Cost { get; set; }
        public bool PayDone { get; set; } //статус платежа
    }

    public class InvoiceDto
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public int TaskId { get; set; }
        public double Cost { get; set; }
        public bool PayDone { get; set; } //статус платежа
    }
}
