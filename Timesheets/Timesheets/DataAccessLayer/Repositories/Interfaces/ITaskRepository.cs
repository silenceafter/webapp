using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.DataAccessLayer.Interfaces;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Repositories.Interfaces
{
    public interface ITaskRepository : IRepository<TaskModel>
    {
        bool RegisterTask(TaskModel task);
        TaskModel GetTask(int id, int contractid, int taskid);
        List<TaskModel> GetTaskAll(int id, int contractid);
    }
}
