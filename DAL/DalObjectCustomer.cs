using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Dal
{
    public partial class DalObject
    {
        public void AddCustomer(Customer newCustomer)
        {
            if (DataSource.Customers.Exists(item => item.Id == newCustomer.Id))
                throw new ExistsException($"Customer id: {newCustomer.Id} already exists.");
            DataSource.Customers.Add(newCustomer);
        }
        public Customer GetCustomer(int idCustomer)
        {
            //search for the customer that has the same id has the id that the user enterd
            int customerIndex = DataSource.Customers.FindIndex(item => item.Id == idCustomer);
            if (customerIndex == -1)
                throw new DoesNotExistException($"Customer id: { idCustomer } does not exists.");
            return DataSource.Customers[customerIndex];
        }
        public IEnumerable<Customer> GetCustomers(Predicate<Customer> predicate = null)
        {
            List<Customer> Customers = new ();
            foreach (var itC in DataSource.Customers)
                if (predicate == null || predicate(itC))
                    Customers.Add(itC);
            return Customers;
        }
        public void UpdateCustomer(int id,string name,string phone)
        {
            int customerIndex = DataSource.Customers.FindIndex(item => item.Id == id);
            Customer customer = DataSource.Customers[customerIndex];
            //meens we want to update the name 
            if (name != "\n")
                customer.Name = name;
            //meens we want to update the phone
            if (phone != "\n")
                customer.Phone = phone;
            DataSource.Customers[customerIndex] = customer;//to change the customer in the list of customers
        }
    }
}
