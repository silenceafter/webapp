using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Requests;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Interfaces
{
    public interface ICustomerRepository
    {
        int RegisterCustomer(CustomerDto customer);
        CustomerDto GetCustomer(int id);
        bool SetCustomer(CustomerDto customer);
        List<CustomerDto> GetCustomerAll();
    }
}
