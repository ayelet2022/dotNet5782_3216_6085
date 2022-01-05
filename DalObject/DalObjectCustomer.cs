using System;
using System.Collections.Generic;
using System.Linq;

using DalApi;
using DO;

namespace Dal
{
    internal sealed partial class DalObject : IDal
    {
        public void AddCustomer(Customer newCustomer)
        {
            if (DataSource.Customers.Exists(item => item.Id == newCustomer.Id))
                throw new ExistsException($"Customer id: {newCustomer.Id} already exists.");
            DataSource.Customers.Add(newCustomer);
        }
        public Customer GetCustomer(int idCustomer)
        {
            try
            {
                //search for the customer that has the same id has the id that the user enterd
                return DataSource.Customers.First(item => item.Id == idCustomer);
            }
            catch (InvalidOperationException ex)
            {
                throw new DoesNotExistException($"Customer id: { idCustomer } does not exists.");
            }
        }
        public IEnumerable<Customer> GetCustomers(Predicate<Customer> predicate = null)
        {
            return from item in DataSource.Customers
                   where predicate == null ? true : predicate(item)
                   select item;
        }
        public void UpdateCustomer(int id,string name,string phone)
        {
            int customerIndex = DataSource.Customers.FindIndex(item => item.Id == id);
            if(customerIndex == -1)
                throw new DoesNotExistException($"Customer id: {id} does not exist.");
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
