using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.DataAccessLayer.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public DateTime Date { get; set; }
    }
}
