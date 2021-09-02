using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.Controllers.Responses
{
    public class EmployeeResponse
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
        public List<TaskDto> Tasks { get; set; } //задания
    }
}
