using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.Controllers.Requests
{
    public class EmployeeRequest
    {
        public double Rate { get; set; } //ставка
        public double BankAccount { get; set; } //счет
    }
}
