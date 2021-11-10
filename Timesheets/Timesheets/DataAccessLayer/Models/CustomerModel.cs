using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.DataAccessLayer.Models
{
    public class CustomerModel
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double BankAccount { get; set; } //счет
        public List<int> Contracts { get; set; }
    }

    public class CustomerDto
    {
        public int Id { get; set; }
        public double BankAccount { get; set; } //счет
    }
}
