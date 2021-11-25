using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Requests;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Services.Interfaces
{
    public interface ICustomerService
    {
        CustomerModel RegisterCustomer(CustomerRequest customer);
        CustomerModel GetCustomer(int id);
        bool SetCustomer(CustomerModel customer);
        List<CustomerModel> GetCustomerAll();
    }
}
