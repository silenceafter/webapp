using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.DataAccessLayer.Models
{
    public class Contract
    {
        public int id { get; set; }
        public double account { get; set; }
        public List<Employee> EmployeesList { get; set; }
        public List<Invoice> InvoicesList { get; set; }
    }
}
