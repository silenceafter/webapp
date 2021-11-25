using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.Controllers.Requests
{
    public class TaskEmployeeRequest
    {
        public int Task { get; set; }
        public int Employee { get; set; }
    }
}
