using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
namespace BL
{
    class BLCustomer
    {
        public void AddCustomer(Customer customer)
        {

        }

        public Customer PrintCustomer(int idCustomer)
        {
            Customer customer = new();
            return customer;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            List<Customer> Customers = new();
            return Customers;
        }
    }
}
