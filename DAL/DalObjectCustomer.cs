﻿using System;
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
                throw new MyException("Customer already exists.");
            DataSource.Customers.Add(newCustomer);
        }

        /// <summary>
        /// search for the customer in the arrey thet has the same id as the user enterd and retirnes it
        /// </summary>
        /// <param name="idCustomer">the id(that was enterd by the user in main) of the customer that the user wants to print</param>
        /// <returns>resturn the customer that needs to be printed</returns>
        public Customer PrintCustomer(int idCustomer)
        {
            if (DataSource.Customers.Exists(item => item.Id != idCustomer))
                throw new MyException("Customer does not exists.");
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
        public int searchCustomer(int id)
        {
            int index = 0;
            foreach (var itC in DataSource.Customers)
            {
                if (itC.Id == id)
                    return index;
                index++;
            }
            return -1;
        }
    }
}
