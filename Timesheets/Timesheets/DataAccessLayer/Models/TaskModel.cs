using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.DataAccessLayer.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public ContractModel Contract { get; set; }
        public List<TaskEmployeeModel> Employees { get; set; }
    }
}
