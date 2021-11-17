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
                    throw new InvalidInputException("The customer id is incorrect");
                if (customer.Name == "\n")
                    throw new InvalidInputException("The customer name is incorrect");
                if(customer.Phone.Length!=10)
                    throw new InvalidInputException("The customer phone is incorrect");
                if (customer.CustomerLocation.Longitude<-180||customer.CustomerLocation.Longitude>180)
                    throw new InvalidInputException("The customer Longitude is incorrect");
                if (customer.CustomerLocation.Latitude < -90 || customer.CustomerLocation.Latitude > 90)
                    throw new InvalidInputException("The customer Latitude is incorrect");
                IDAL.DO.Customer newCustomer = new();
                newCustomer.CopyPropertiesTo(newCustomer);
                dal.AddCustomer(newCustomer);
            }
            catch (IDAL.DO.ExistsException ex)
            {
                throw new FailedToAddException(ex.ToString(), ex);
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
