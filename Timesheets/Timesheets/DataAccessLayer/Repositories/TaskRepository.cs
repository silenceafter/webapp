using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.DataAccessLayer.Interfaces;
using Timesheets.DataAccessLayer.Models;
using Timesheets.DataAccessLayer.Repositories.Interfaces;

namespace Timesheets.DataAccessLayer.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly DictionariesGlobal _dictionaryGlobal;
        private IContractRepository _contractRepository;

        public TaskRepository(DictionariesGlobal dictionariesGlobal, IContractRepository contractRepository)
        {
            _dictionaryGlobal = dictionariesGlobal;
            _contractRepository = contractRepository;
        }

        public bool RegisterTask(TaskModel task)
        {
            _dictionaryGlobal.tasks.Add(task); //сохраняем задачу
            ContractModel contract = task.Contract;
            //
            if (contract != null)
            {
                contract.Tasks.Add(task); //копия у кастомера                  
            }

            _dictionaryGlobal._taskid++;
            return true;
        }

        public TaskModel GetTask(int id, int contractid, int taskid)
        {
            TaskModel currentTask = null;
            ContractModel contract = _contractRepository.GetContract(id, contractid);
            if (contract != null)
            {
                //контракт найден
                var tasks = contract.Tasks;
                foreach (var task in tasks)
                {
                    if (task.Id.Equals(taskid))
                    {
                        //задача найдена
                        currentTask = task;
                        break;
                    }
                }
            }
            return currentTask;
        }

        public List<TaskModel> GetTaskAll(int id, int contractid)
        {
            List<TaskModel> tasks = null;
            ContractModel contract = _contractRepository.GetContract(id, contractid);
            if (contract != null)
            {
                //контракт найден, берем задачиы
                tasks = contract.Tasks;
            }
            return tasks;
        }
    }
}
