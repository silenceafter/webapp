using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Timesheets.Data;
using Timesheets.DataAccessLayer.Interfaces;
using Timesheets.DataAccessLayer.Models;
using Timesheets.DataAccessLayer.Repositories.Interfaces;
using Timesheets.DataAccessLayer.Services.Interfaces;

namespace Timesheets.DataAccessLayer.Repositories
{
    public class TaskRepository : ITaskRepository
    {      
        private TimesheetContext _context;
        private readonly ILogger<TaskRepository> _logger;

        public TaskRepository(
            TimesheetContext context,
            ILogger<TaskRepository> logger
            )
        {
            _context = context;
            _logger = logger;
        }
        
        public int RegisterTask(TaskDto task)
        {
            _logger.LogInformation("RegisterTask() запуск метода");
            if (task != null)
            {
                try
                {
                    _context.Tasks.Add(task);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"RegisterTask() ошибка, {ex.Message}");
                }
                return task.Id;
            }
            return 0;
        }

        public TaskDto GetTask(int id, int contractid, int taskid)
        {
            _logger.LogInformation("GetTask() запуск метода");
            return _context.Tasks
                .Where(row => row.Id == taskid)
                .Where(row => row.ContractId == contractid)
                .SingleOrDefault();
        }

        public TaskDto GetTask(int taskid)
        {
            _logger.LogInformation("GetTask() запуск метода");
            return _context.Tasks
                .Where(row => row.Id == taskid)
                .SingleOrDefault();
        }

        public List<TaskDto> GetTaskAll(int contractid)
        {
            _logger.LogInformation("GetTaskAll() запуск метода");
            return _context.Tasks
                .Where(row => row.ContractId == contractid)
                .ToList();
        }
    }
}
