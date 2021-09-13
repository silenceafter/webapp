using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Responses;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets
{
    public class DictionariesGlobal
    {
        public int _customerid;
        public int _contractid;
        public int _invoiceid;
        public int _taskid;
        public int _employeeid;
        public int _taskEmployeeId;
        public List<CustomerModel> customers { get; set; }
        public List<ContractModel> contracts { get; set; }
        public List<InvoiceModel> invoices { get; set; }
        public List<TaskModel> tasks { get; set; }
        public List<EmployeeModel> employees { get; set; }

        public DictionariesGlobal()
        {
            //customers
            _customerid = 1;
            customers = new List<CustomerModel>();
            //contracts
            _contractid = 1;
            contracts = new List<ContractModel>();
            //invoices
            _invoiceid = 1;
            invoices = new List<InvoiceModel>();
            //tasks
            _taskid = 1;
            tasks = new List<TaskModel>();
            //employee
            _employeeid = 1;
            employees = new List<EmployeeModel>();
            //taskEmployee
            _taskEmployeeId = 1;
        }
    }
}
