using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.Controllers.Responses
{
    public class ContractResponse
    {
        public int Id { get; set; }
        public int Customer { get; set; }
        public List<int> Tasks { get; set; }
        public List<int> Invoices { get; set; } //можно посчитать стоимость контракта
    }
}
