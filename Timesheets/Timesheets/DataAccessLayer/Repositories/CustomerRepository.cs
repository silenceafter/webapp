using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Requests;
using Timesheets.Data;
using Timesheets.DataAccessLayer.Interfaces;
using Timesheets.DataAccessLayer.Models;
using Timesheets.DataAccessLayer.Services.Interfaces;

namespace Timesheets.DataAccessLayer.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private TimesheetContext _context;
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(
            TimesheetContext context,
            ILogger<CustomerRepository> logger
            )
        {
            _context = context;
            _logger = logger;
        }

        public int RegisterCustomer(CustomerDto customer)
        {
            _logger.LogInformation("RegisterCustomer() запуск метода");
            if (customer != null)
            {
                try
                {
                    _context.Customers.Add(customer);
                    _context.SaveChanges();
                }
                catch(Exception ex)
                {
                    _logger.LogError($"RegisterCustomer() ошибка, {ex.Message}");
                }
                return customer.Id;
            }
            return 0;
        }

        public CustomerDto GetCustomer(int id)
        {
            _logger.LogInformation("GetCustomer() запуск метода");
            return _context.Customers.Where(row => row.Id == id).SingleOrDefault();
        }

        public bool SetCustomer(CustomerDto customer)
        {
            _logger.LogInformation("SetCustomer() запуск метода");
            if (customer != null)
            {
                try
                {
                    var response = this.GetCustomer(customer.Id);
                    if (response != null)
                    {
                        response.BankAccount = customer.BankAccount;
                    }
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"SetCustomer() ошибка, {ex.Message}");
                }
                return true;
            }
            return false;
        }

        public List<CustomerDto> GetCustomerAll()
        {
            _logger.LogInformation("GetCustomerAll() запуск метода");
            return _context.Customers.ToList();
        }        
    }
}
