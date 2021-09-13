using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Responses;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Services.Interfaces
{
    public interface ITaskService
    {
        TaskResponse RegisterTask(TaskModel task);
        TaskResponse GetTask(int id, int contractid, int taskid);
        List<TaskResponse> GetTaskAll(int id, int contractid);
        TaskModel GetModel(int id, int contractid, int taskid);
    }
}
