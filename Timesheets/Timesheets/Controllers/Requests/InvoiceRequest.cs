using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.Controllers.Requests
{
    public class InvoiceRequest
    {
        public int Contract { get; set; }
        public int Task { get; set; }
        public double Cost { get; set; }
    }
}
