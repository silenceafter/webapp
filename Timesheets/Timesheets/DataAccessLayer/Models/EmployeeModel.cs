using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        public double Rate { get; set; } //ставка
        public double BankAccount { get; set; } //счет
        public List<int> Tasks { get; set; } //задания
    }

    public class EmployeeDto
    {
        public int Id { get; set; }
        public double Rate { get; set; } //ставка
        public double BankAccount { get; set; } //счет
    }
}
