using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.DataAccessLayer.Models
{
    public class Client
    {
        public int id { get; set; }
        public double account { get; set; }//денежный счет для оплаты работ
        public List<Contract> ContractsList { get; set; }
    }
}
