using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.Controllers.Responses
{
    public class CustomerResponse
    {
        public int Id { get; set; }
        public double BankAccount { get; set; } //счет      
        public List<int> Contracts { get; set; } //только id
    }

    public class CustomerDto
    {
        public int Id { get; set; }  
        public double BankAccount { get; set; } //счет      
        public List<ContractDto> Contracts { get; set; }
    }
}
