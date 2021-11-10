using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.DataAccessLayer.Models
{
    public class ContractModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public List<int> Tasks { get; set; }
        public List<int> Invoices { get; set; } //можно посчитать стоимость контракта
    }

    public class ContractDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
    }
}
