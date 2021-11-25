using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Requests;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Services.Interfaces
{
    public interface ITaskService
    {
        TaskModel RegisterTask(TaskRequest task);
        TaskModel GetTask(int customerid, int contractid, int taskid);
        TaskModel GetTask(int taskid);
        List<TaskModel> GetTaskAll(int contractid);        
    }
}
