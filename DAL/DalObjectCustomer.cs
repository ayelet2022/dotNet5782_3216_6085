using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
namespace DalObject
{
    public partial class DalObject
    {
        /// <summary>
        /// adds a new customer to the arrey
        /// </summary>
        /// <param name="newCustomer">the new customer that the user entered in main and needs to be added to the arrey</param>
        public void AddCustomer(Customer newCustomer)
        {
            if (DataSource.Customers.Exists(item => item.Id == newCustomer.Id))
                throw new ExistsException("Customer already exists.");
            DataSource.Customers.Add(newCustomer);
        }

        /// <summary>
        /// search for the customer in the arrey thet has the same id as the user enterd and retirnes it
        /// </summary>
        /// <param name="idCustomer">the id(that was enterd by the user in main) of the customer that the user wants to print</param>
        /// <returns>resturn the customer that needs to be printed</returns>
        public Customer GetCustomer(int idCustomer)
        {
            if (DataSource.Customers.Exists(item => item.Id != idCustomer))
                throw new DoesNotExistException("Customer does not exists.");
            int customerIndex = 0;
            while (DataSource.Customers[customerIndex].Id != idCustomer)//search for the customer that has the same id has the id that the user enterd
                customerIndex++;
            return (DataSource.Customers[customerIndex]);
        }

        public IEnumerable<Customer> GetCustomers()
        {
            List<Customer> Customers = new ();
            foreach (var itC in DataSource.Customers)
            {
                Customers.Add(itC);
            }
            return Customers;
        }
        public void UpdateCustomer(int id,string name,string phone)
        {
            int i = 0;
            Customer customer = new();
            foreach (var item in DataSource.Customers)
            {
                if (item.Id == id)
                {
                    customer = item;
                    if (name != "\n")
                        customer.Name = name;
                    if (phone != "\n")
                        customer.Phone = phone;
                    break;
                }
                i++;
            }
            DataSource.Customers[i] = customer;
        }
    }
}
