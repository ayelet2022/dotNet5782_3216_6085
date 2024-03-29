﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DalApi;
using DO;

namespace Dal
{
    internal sealed partial class DalObject : IDal
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(Customer newCustomer)
        {
            if (DataSource.Customers.Exists(item => item.Id == newCustomer.Id && item.IsActive))
                throw new ExistsException($"Customer id: {newCustomer.Id} already exists.");
            newCustomer.IsActive = true;
            DataSource.Customers.Add(newCustomer);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetCustomers(Predicate<Customer> predicate = null)
        {
            return from item in DataSource.Customers
                   where predicate == null ? true : predicate(item)
                   select item;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(int id,string name,string phone)
        {
            int customerIndex = DataSource.Customers.FindIndex(item => item.Id == id && item.IsActive);
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteCustomer(int id)
        {
            try
            {
                Customer customer = GetCustomer(id);
                customer.IsActive = false;
                int customerIndex = DataSource.Customers.FindIndex(item => item.Id == id);
                DataSource.Customers[customerIndex] = customer;
            }
            catch (DoesNotExistException ex)
            {
                throw new ItemIsDeletedException($"Customer: { id } is already deleted.");
            }

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool IsActive(int id)
        {
            Customer customer = GetCustomer(id);
            if (customer.IsActive)
                return true;
            return false;
        }

    }
}
