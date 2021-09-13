using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.DataAccessLayer.Models
{
    public class TaskEmployeeModel
    {
        public int Id { get; set; }
        public TaskModel Task { get; set; }
        public EmployeeModel Employee { get; set; }
        public bool Done { get; set; }
        public double Hours { get; set; }
    }
}
