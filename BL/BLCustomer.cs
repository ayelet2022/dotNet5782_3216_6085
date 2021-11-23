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
        /// <summary>
        /// adds a new customer to the list of customers
        /// </summary>
        /// <param name="customer">the new customer we want to add</param>
        public void AddCustomer(Customer customer)
        {
            try
            {
                if (customer.Id < 100000000 || customer.Id > 999999999)
                    throw new InvalidInputException("The customer id is incorrect");
                if (customer.Name == "\n")
                    throw new InvalidInputException("The customer name is incorrect");
                if (customer.Phone.Length != 10)
                    throw new InvalidInputException("The customer phone is incorrect");              
                if (customer.CustomerLocation.Longitude < 31.7082 || customer.CustomerLocation.Longitude > 32.1027879)
                    throw new InvalidInputException("The customer Longitude is incorrect");
                if (customer.CustomerLocation.Latitude < 34.75948 || customer.CustomerLocation.Latitude > 35.2642)
                    throw new InvalidInputException("The customer Latitude is incorrect");
                IDAL.DO.Customer newCustomer = new();
                object obj = newCustomer;
                customer.CopyPropertiesTo(obj);
                newCustomer = (IDAL.DO.Customer)obj;
                newCustomer.Longitude = customer.CustomerLocation.Longitude;
                newCustomer.Latitude = customer.CustomerLocation.Latitude;
                dal.AddCustomer(newCustomer);
            }
            catch (IDAL.DO.ExistsException ex)
            {
                throw new FailedToAddException(ex.ToString(), ex);
            }
        }

        /// <summary>
        /// returne the customer with the id that was enterd
        /// </summary>
        /// <param name="idCustomer">the id of the customer we want to returne</param>
        /// <returns>the customer with the id</returns>
        public Customer GetCustomer(int idCustomer)
        {
            try
            {
                IDAL.DO.Customer dalCustomer = dal.GetCustomers().First(item => item.Id == idCustomer);
                Customer customer = new();//the customer to returne
                dalCustomer.CopyPropertiesTo(customer);//only puts:id,name,phone,location
                customer.CustomerLocation = new();
                customer.CustomerLocation.Latitude = dalCustomer.Latitude;
                customer.CustomerLocation.Longitude = dalCustomer.Longitude;
                foreach (var item in dal.GetParcels())//checks all the parcels in dal
                {
                    //check if the parcel is from this customer or to this customer
                    if (item.SenderId == idCustomer || item.TargetId == idCustomer)
                    {
                        ParcelInCustomer parcelFromCustomer = new();
                        item.CopyPropertiesTo(parcelFromCustomer);//only copies:id,weight,priority
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
                        parcelFromCustomer.SenderOrRecepter = new();
                        parcelFromCustomer.SenderOrRecepter.Id = customer.Id;
                        parcelFromCustomer.SenderOrRecepter.Name = customer.Name;
                        customer.ParcelsFromCustomers = new();
                        customer.ParcelsToCustomers = new();
                        if (item.SenderId == idCustomer)//meens the parcel is from the customer
                            customer.ParcelsFromCustomers.Add(parcelFromCustomer);//adds the parcel that this customer send
                        if (item.TargetId == idCustomer)//meens the parcel to the customer
                            customer.ParcelsToCustomers.Add(parcelFromCustomer);//adds the parcel that this customer send
                    }
                }
                return customer;
            }
            catch (InvalidOperationException ex)
            {
                throw new NotFoundInputException($"A customer with the id:{idCustomer} was not found", ex);
            }
        }

        /// <summary>
        /// returne the all list of the customers
        /// </summary>
        /// <returns>the new list of the customers</returns>
        public IEnumerable<CustomerList> GetCustomers()
        {
            CustomerList customer = new();
            List<CustomerList> Customers = new();//the customer list that we whant to returne
            foreach (var item in dal.GetCustomers())
            {
                item.CopyPropertiesTo(customer);//copy only:id,name,phone
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
                customer = new();
            }
            return Customers;
        }
        /// <summary>
        /// updates the customer with the id that was enterd
        /// </summary>
        /// <param name="id">the id of the customet we want to update</param>
        /// <param name="name">the new name to the customer</param>
        /// <param name="phone">the new phone to the customer</param>
        public void UpdateCustomer(int id, string name, string phone)
        {
            try
            {
                if (phone!="\n" && phone.Length < 10)//meens the phone is incurrect
                    throw new InvalidInputException($"The customer phone{phone} is incorrect");
                dal.GetCustomer(id);//fineds the customer with the id
                dal.UpdateCustomer(id, name, phone);//to update the customr
            }
            catch (IDAL.DO.ExistsException ex)//meens ther is no customer with the id that was enterd
            {
                throw new FailToUpdateException(ex.ToString(), ex);
            }
        }
        /// <summary>
        /// returne the distance between the sender of the parcel to the resever of the parcel
        /// </summary>
        /// <param name="sender">the customer that send the parcel</param>
        /// <param name="resever">the customer that suppsed to resev the parcel</param>
        /// <returns></returns>
        public double DisSenderToResever(Customer sender, Customer resever)
        {
            return Distance.Haversine
                (sender.CustomerLocation.Longitude, sender.CustomerLocation.Latitude, resever.CustomerLocation.Longitude, resever.CustomerLocation.Latitude);
        }
        /// <summary>
        /// fineds the closest base station to the customer
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns>the closest base station</returns>
        public IDAL.DO.BaseStation FindMinDistanceOfCToBS(double latitude, double longitude)
        {
            IDAL.DO.BaseStation baseStation = new();
            double minDistance = 0;
            double distance = 0;
            //to go over all the station and check wich one is the closest
            foreach (var item in dal.GetBaseStations())
            {
                //the distatce between the customer to the base station
                distance = Distance.Haversine(item.Latitude, item.Longitude,latitude,longitude);
                //if the distance of this station in smaller then the one before
                if (minDistance == 0 || minDistance > distance)
                {
                    minDistance = distance;
                    baseStation = item;
                }
            }
            return baseStation;
        }
    }
}

