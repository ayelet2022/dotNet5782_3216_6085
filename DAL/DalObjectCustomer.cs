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
        public IEnumerable<Customer> GetCustomers()
        {
            List<Customer> Customers = new ();
            foreach (var itC in DataSource.Customers)
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
        public IEnumerable<int> GetCustomersRe()
        {
            List<int> customersList= new List<int>();
            //to go over all the list of parcels
            foreach (var item in DataSource.Parcels)
            {
                if (item.Delivered != null)//meens the parcel was deliverd
                    customersList.Add(item.TargetId);//adds the customer that reseved the parcel
            }
            return customersList;
        }
    }
}
