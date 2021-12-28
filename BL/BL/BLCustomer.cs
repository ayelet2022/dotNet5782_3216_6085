using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
namespace BL
{
    public partial class BL
    {
        public void AddCustomer(Customer customer)
        {
           if (customer.Id < 100000000 || customer.Id > 999999999)
               throw new InvalidInputException($"The customer id: {customer.Id} is incorrect, the customer wasn't added.\n");
           if (customer.Name == "\n")
               throw new InvalidInputException($"The customer name is incorrect, the customer wasn't added.\n");
           if (customer.Phone.Length != 10)
               throw new InvalidInputException($"The customer phone: {customer.Phone} is incorrect, the customer wasn't added.\n");              
           if (customer.CustomerLocation.Longitude < 34|| customer.CustomerLocation.Longitude > 37)
               throw new InvalidInputException($"The customer Longitude: {customer.CustomerLocation.Longitude} is incorrect, the customer wasn't added.\n");
           if (customer.CustomerLocation.Latitude <30 || customer.CustomerLocation.Latitude > 33)
               throw new InvalidInputException($"The customer Latitude: {customer.CustomerLocation.Latitude} is incorrect, the customer wasn't added.\n");
            try
            {
                DO.Customer newCustomer = new();
                object obj = newCustomer;
                customer.CopyPropertiesTo(obj);
                newCustomer = (DO.Customer)obj;
                newCustomer.Longitude = customer.CustomerLocation.Longitude;
                newCustomer.Latitude = customer.CustomerLocation.Latitude;
                dal.AddCustomer(newCustomer);
            }
            catch (DO.ExistsException ex)
            {
                throw new FailedToAddException($"The customer: {customer.Id} already exists.", ex);
            }
        }

        public Customer GetCustomer(int idCustomer)
        {
            try
            {
                DO.Customer dalCustomer = dal.GetCustomer(idCustomer);
                Customer customer = new();//the customer to returne
                dalCustomer.CopyPropertiesTo(customer);//only puts:id,name,phone,location
                customer.CustomerLocation = new();
                customer.ParcelsFromCustomers = new();
                customer.ParcelsToCustomers = new();
                customer.CustomerLocation.Latitude = dalCustomer.Latitude;
                customer.CustomerLocation.Longitude = dalCustomer.Longitude;
                //check if the parcel is from this customer or to this customer
                foreach (var item in dal.GetParcels(item=> item.SenderId == idCustomer || item.TargetId == idCustomer))//checks all the parcels in dal
                {
                    ParcelInCustomer parcelFromCustomer = new  ParcelInCustomer    ();
                    item.CopyPropertiesTo(parcelFromCustomer);//only copies:id,weight,priority
                    if (item.Delivered != null)//meens the parcel was deliverd
                        parcelFromCustomer.Status = ParcelStatus.delivery;//the parcel was deliverd
                    else//meens the parcel is pickedup/scheduled/created
                    {
                        if (item.PickedUp != null)//meens the parcel was picked up by the drone
                            parcelFromCustomer.Status = ParcelStatus.pickup;//the parcel was pickedup by the drone
                        else//meens the parcel is scheduled/created
                        {
                            if (item.Scheduled != null)//meens the parcel was scheduled
                                parcelFromCustomer.Status = ParcelStatus.schedul;//the parcel was scheduled
                            else//meens the parcel is created
                                parcelFromCustomer.Status = ParcelStatus.creat;//the parcel was created
                        }
                    }
                    parcelFromCustomer.SenderOrRecepter = new();
                    parcelFromCustomer.SenderOrRecepter.Id = customer.Id;
                    parcelFromCustomer.SenderOrRecepter.Name = customer.Name;
                    if (item.SenderId == idCustomer)//meens the parcel is from the customer
                        customer.ParcelsFromCustomers.Add(parcelFromCustomer);//adds the parcel that this customer send
                    if (item.TargetId == idCustomer)//meens the parcel to the customer
                        customer.ParcelsToCustomers.Add(parcelFromCustomer);//adds the parcel that this customer send
                }
                return customer;
            }
            catch (DO.DoesNotExistException ex)
            {
                throw new NotFoundInputException($"The customer: {idCustomer} was not found.\n", ex);
            }
        }

        public IEnumerable<CustomerList> GetCustomers(Predicate<CustomerList> predicate = null)
        {
            CustomerList customer = new();
            List<CustomerList> Customers = new();//the customer list that we whant to returne
            foreach (var item in dal.GetCustomers())
            {
                item.CopyPropertiesTo(customer);//copy only:id,name,phone
                foreach (var item1 in dal.GetParcels(x => x.SenderId == item.Id || x.TargetId == item.Id))
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
            return Customers.FindAll(item => predicate == null ? true : predicate(item));
        }

        public void UpdateCustomer(int id, string name, string phone)
        {
            try
            {
                dal.GetCustomer(id);//fineds the customer with the id
                if (phone!="\n" && phone.Length < 10)//meens the phone is incurrect
                    throw new InvalidInputException($"The customer phone: {phone} is incorrect");
                dal.UpdateCustomer(id, name, phone);//to update the customr
            }
            catch (NotFoundInputException ex)
            {
                throw new FailToUpdateException($"The customer: {id} wasn't found, the customer wasn't updated.\n", ex);
            }
            catch (InvalidInputException ex)
            {
                throw new FailToUpdateException($"The phone: {phone} is incorrect, the customer: {id} wasn't updated.\n", ex);
            }
        }

        public double DisSenderToResever(Customer sender, Customer resever)
        {
            return Distance.Haversine
                (sender.CustomerLocation.Longitude, sender.CustomerLocation.Latitude, resever.CustomerLocation.Longitude, resever.CustomerLocation.Latitude);
        }

        public DO.BaseStation FindMinDistanceOfCToBS(double latitude, double longitude)
        {
            DO.BaseStation baseStation = new();
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

