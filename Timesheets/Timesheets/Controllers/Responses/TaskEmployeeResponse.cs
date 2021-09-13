using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.Controllers.Responses
{
    public class TaskEmployeeResponse
    {
        public int Id { get; set; }
        public int Task { get; set; }
        public int Employee { get; set; }
        public bool Done { get; set; }
        public double Hours { get; set; }
    }
}
