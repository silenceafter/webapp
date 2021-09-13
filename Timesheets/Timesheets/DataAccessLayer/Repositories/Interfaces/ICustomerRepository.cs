using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Interfaces
{
    public interface ICustomerRepository : IRepository<CustomerModel>
    {
        bool RegisterCustomer(CustomerModel customer);
        CustomerModel GetCustomer(int id);
        List<CustomerModel> GetCustomerAll();
    }
}
