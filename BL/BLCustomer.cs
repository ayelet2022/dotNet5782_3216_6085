using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
namespace BL
{
    public partial class BL
    {
        public void AddCustomer(Customer customer)
        {
            try
            {
                if (customer.Id < 100000000 || customer.Id > 999999999)
                    throw new InvalidCastException("The customer id is incorrect");
            }
            catch (IDAL.DO.)
            {
                throw;
            }
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
