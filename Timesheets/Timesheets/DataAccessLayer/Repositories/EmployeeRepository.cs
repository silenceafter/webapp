using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Timesheets.Data;
using Timesheets.DataAccessLayer.Interfaces;
using Timesheets.DataAccessLayer.Models;
using Timesheets.DataAccessLayer.Repositories.Interfaces;

namespace Timesheets.DataAccessLayer.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private TimesheetContext _context;
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(
            TimesheetContext context,
            ILogger<EmployeeRepository> logger
            )
        {
            _context = context;
            _logger = logger;
        }

        public int RegisterEmployee(EmployeeDto employee)
        {
            _logger.LogInformation("RegisterEmployee() запуск метода");
            if (employee != null)
            {
                try
                {
                    _context.Employees.Add(employee);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"RegisterEmployee() ошибка, {ex.Message}");
                }
                return employee.Id;
            }
            return 0;
        }

        public EmployeeDto GetEmployee(int id)
        {
            _logger.LogInformation("GetEmployee() запуск метода");
            return _context.Employees.Where(row => row.Id == id).SingleOrDefault();
        }

        public bool SetEmployee(EmployeeDto employee)
        {
            _logger.LogInformation("SetEmployee() запуск метода");
            if (employee != null)
            {
                try
                {
                    var response = this.GetEmployee(employee.Id);
                    if (response != null)
                    {
                        response.BankAccount = employee.BankAccount;
                    }
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"SetEmployee() ошибка, {ex.Message}");
                }
                return true;
            }
            return false;
        }

        public List<EmployeeDto> GetEmployeeAll()
        {
            _logger.LogInformation("GetEmployeeAll() запуск метода");
            return _context.Employees.ToList();
        }
    }
}