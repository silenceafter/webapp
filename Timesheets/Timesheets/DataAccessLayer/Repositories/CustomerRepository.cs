using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Responses;
using Timesheets.DataAccessLayer.Interfaces;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DictionariesGlobal _dictionaryGlobal;
        public CustomerRepository(DictionariesGlobal dictionariesGlobal)
        {
            _dictionaryGlobal = dictionariesGlobal;
        }

        public bool RegisterCustomer(CustomerModel customer)
        {
            var customers = _dictionaryGlobal.customers;
            var customerId = _dictionaryGlobal._customerid;
            bool done = false;
            try
            {
               if(customer != null)
                {
                    customers.Add(customer);
                    _dictionaryGlobal._customerid++;
                    done = true;
                }                
            }
            catch (Exception ex)
            {
               //nlog 
            }
            return done;
        }

        public CustomerModel GetCustomer(int id)
        {
            var customers = _dictionaryGlobal.customers;
            CustomerModel currentCustomer = null;           
            foreach (var customer in customers)
            {
                if (customer.Id.Equals(id))
                {
                    //кастомер найден
                    currentCustomer = customer;            
                    break;
                }
            }
            return currentCustomer;
        }

        public List<CustomerModel> GetCustomerAll()
        {
            var customers = _dictionaryGlobal.customers;            
            return customers;
        }        
    }
}
