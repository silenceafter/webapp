using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Responses;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Services.Interfaces
{
    public interface ICustomerService
    {
        CustomerResponse RegisterCustomer(CustomerModel customer);
        CustomerResponse GetCustomer(int id);
        List<CustomerResponse> GetCustomerAll();
        CustomerModel GetModel(CustomerResponse customerResponse);
    }
}
