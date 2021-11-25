using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.DataAccessLayer.Interfaces;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Repositories.Interfaces
{
    public interface ITaskRepository// : IRepository<TaskDto>
    {
        int RegisterTask(TaskDto task);
        TaskDto GetTask(int id, int contractid, int taskid);
        TaskDto GetTask(int taskid);
        List<TaskDto> GetTaskAll(int contractid);
    }
}
