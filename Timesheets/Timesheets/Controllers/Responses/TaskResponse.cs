using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.Controllers.Responses
{
    public class TaskResponse
    {
        public int Id { get; set; }
        public int Contract { get; set; }
        public List<int> Employees { get; set; } //список работников, прикрепленных к задаче, не список сущностей
    }
}
