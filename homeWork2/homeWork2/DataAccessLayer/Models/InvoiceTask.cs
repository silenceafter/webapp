using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.DataAccessLayer.Models
{
    public class InvoiceTask
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int TaskId { get; set; }
    }
}
