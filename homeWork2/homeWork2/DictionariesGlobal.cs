using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Responses;

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
        public List<CustomerDto> customers { get; set; }
        public List<ContractDto> contracts { get; set; }
        public List<InvoiceDto> invoices { get; set; }
        public List<TaskDto> tasks { get; set; }
        public List<EmployeeDto> employees { get; set; }

        public DictionariesGlobal()
        {
            //customers
            _customerid = 1;
            customers = new List<CustomerDto>();
            //contracts
            _contractid = 1;
            contracts = new List<ContractDto>();
            //invoices
            _invoiceid = 1;
            invoices = new List<InvoiceDto>();
            //tasks
            _taskid = 1;
            tasks = new List<TaskDto>();
            //employee
            _employeeid = 1;
            employees = new List<EmployeeDto>();
            //taskEmployee
            _taskEmployeeId = 1;
        }
    }
}
