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

        public Customer GetCustomer(int idCustomer)
        {
            IDAL.DO.Customer dalCustomer = dal.GetCustomers().First(item => item.Id == idCustomer);
            Customer customer = new();//the customer to returne
            customer.CopyPropertiesTo(dalCustomer);//only puts:id,name,phone,location
            foreach (var item in dal.GetParcels())//checks all the parcels in dal
            {
                //check if the parcel is from this customer or to this customer
                if (item.SenderId == idCustomer|| item.TargetId == idCustomer)
                {
                    ParcelInCustomer parcelFromCustomer = new();
                    parcelFromCustomer.CopyPropertiesTo(item);//only copies:id,weight,priority
                    if (item.Delivered != DateTime.MinValue)//meens the parcel was deliverd
                        parcelFromCustomer.Status = (ParcelStatus)3;//the parcel was deliverd
                    else//meens the parcel is pickedup/scheduled/created
                    {
                        if (item.PickedUp != DateTime.MinValue)//meens the parcel was picked up by the drone
                            parcelFromCustomer.Status = (ParcelStatus)2;//the parcel was pickedup by the drone
                        else//meens the parcel is scheduled/created
                        {
                            if (item.Scheduled != DateTime.MinValue)//meens the parcel was scheduled
                                parcelFromCustomer.Status = (ParcelStatus)1;//the parcel was scheduled
                            else//meens the parcel is created
                                parcelFromCustomer.Status = (ParcelStatus)0;//the parcel was created
                        }
                    }
                    parcelFromCustomer.SenderOrRecepter.Id = customer.Id;
                    parcelFromCustomer.SenderOrRecepter.CustomerName = customer.Name;
                    if(item.SenderId == idCustomer)//meens the parcel is from the customer
                        customer.ParcelsFromCustomers.Add(parcelFromCustomer);//adds the parcel that this customer send
                    if(item.TargetId == idCustomer)//meens the parcel to the customer
                        customer.ParcelsToCustomers.Add(parcelFromCustomer);//adds the parcel that this customer send
                }
            }
            return customer;
        }

        public IEnumerable<CustomerList> GetCustomers()
        {
            CustomerList customer = new();
            List<CustomerList> Customers = new();//the customer list that we whant to returne
            foreach (var item in dal.GetCustomers())
            {
                customer.CopyPropertiesTo(item);//copy only:id,name,phone
                foreach (var item1 in dal.GetParcels())
                {
                    if (item1.SenderId == item.Id)//meens the customer send the parcel
                    {
                        if (item1.Delivered != DateTime.MinValue)//meens the parcel was deliverd
                            customer.ParcelsSentAndDel++;
                        else//meens the parcel was not deliverd
                            customer.ParcelsSentAndNotDel++;
                    }
                    if (item1.TargetId == item.Id)//meens the customer has a parcel on the way/deliverd to him
                    {
                        if (item1.Delivered != DateTime.MinValue)//meens the parcel was deliverd to the customer
                            customer.ParcelsResepted++;
                        else//check if the parcel is on the way
                            if (item1.PickedUp != DateTime.MinValue)//meens the parcel is on the way
                                customer.ParcelsOnTheWay++;
                    }
                }
                Customers.Add(customer);
            }
            return Customers;
        }
        public void UpdateCustomer(int id, string name, string phone)
        {
            if (phone.Length < 10)
                throw new InvalidInputException($"The customer phone is incorrect");
            dal.UpdateCustomer(id, name, phone);
        }
    }
}

