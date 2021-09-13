using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.Controllers.Responses
{
    public class InvoiceResponse
    {
        public int Id { get; set; }
        public int Contract { get; set; }
        public int Task { get; set; }
        public double Cost { get; set; }
        public bool PayDone { get; set; } //статус платежа
    }

    public class InvoiceDto
    {
        public int Id { get; set; }
        public ContractDto Contract { get; set; }
        public TaskDto Task { get; set; }
        public double Cost { get; set; }
        public bool PayDone { get; set; } //статус платежа
    }
}
