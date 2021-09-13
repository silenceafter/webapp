using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.DataAccessLayer.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public double BankAccount { get; set; } //счет      
        public List<ContractModel> Contracts { get; set; }
    }
}
