using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.DataAccessLayer.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public List<int> TaskEmployees { get; set; }
    }

    public class TaskDto
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public int TaskEmployeeId { get; set; }
    }
}
