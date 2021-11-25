using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.DataAccessLayer.Models
{
    public class TaskEmployeeModel
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int EmployeeId { get; set; }
        public bool Done { get; set; }
        public double Hours { get; set; }
    }

    public class TaskEmployeeDto
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int EmployeeId { get; set; }
        public bool Done { get; set; }
        public double Hours { get; set; }
    }
}
