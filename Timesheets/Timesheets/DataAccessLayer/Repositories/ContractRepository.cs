using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Timesheets.Data;
using Timesheets.DataAccessLayer.Interfaces;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private TimesheetContext _context;
        private readonly ILogger<ContractRepository> _logger;

        public ContractRepository(
            TimesheetContext context,            
            ILogger<ContractRepository> logger
            )
        {
            _context = context;
            _logger = logger;
        }

        public int RegisterContract(ContractDto contract)
        {
            _logger.LogInformation("RegisterContract() запуск метода");
            if (contract != null)
            {
                try
                {
                    _context.Contracts.Add(contract);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"RegisterContract() ошибка, {ex.Message}");
                }
                return contract.Id;
            }
            return 0;
        }

        public ContractDto GetContract(int id, int contractid)
        {
            _logger.LogInformation("GetContract() запуск метода");
            return _context.Contracts
                .Where(row => row.Id == contractid)
                .Where(row => row.CustomerId == id)
                .SingleOrDefault();
        }

        public ContractDto GetContract(int contractid)
        {
            _logger.LogInformation("GetContract() запуск метода");
            return _context.Contracts
                .Where(row => row.Id == contractid)
                .SingleOrDefault();
        }

        public List<ContractDto> GetContractAll(int id)
        {
            _logger.LogInformation("GetContractAll() запуск метода");
            return _context.Contracts
                .Where(row => row.CustomerId == id)
                .ToList();
        }
    }
}
