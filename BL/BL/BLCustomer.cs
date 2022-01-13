using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using BO;
namespace BL
{
    public partial class BL
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(Customer customer)
        {
            lock (dal)
            {
                if (customer.Id < 100000000 || customer.Id > 999999999)
                    throw new InvalidInputException($"The customer id: {customer.Id} is incorrect, the customer wasn't added.\n");
                if (customer.Name == "\n")
                    throw new InvalidInputException($"The customer name is incorrect, the customer wasn't added.\n");
                if (customer.Phone.Length != 10)
                    throw new InvalidInputException($"The customer phone: {customer.Phone} is incorrect, the customer wasn't added.\n");
                if (customer.CustomerLocation.Longitude < 34 || customer.CustomerLocation.Longitude > 37)
                    throw new InvalidInputException($"The customer Longitude: {customer.CustomerLocation.Longitude} is incorrect, the customer wasn't added.\n");
                if (customer.CustomerLocation.Latitude < 30 || customer.CustomerLocation.Latitude > 33)
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
        }
        private ParcelInCustomer NewMethod(DO.Parcel indexOfParcels, Customer blCustomer)
        {
           
            ParcelInCustomer parcelAtCustomer = new ParcelInCustomer();
            indexOfParcels.CopyPropertiesTo(parcelAtCustomer);// converting dal->bl
                                                              //If the customer we want is either the sender or the recipient of the package
            if (indexOfParcels.SenderId == blCustomer.Id || indexOfParcels.TargetId == blCustomer.Id)
            {
                if (indexOfParcels.Scheduled != null)//if parcel is assigned a drones
                {
                    if (indexOfParcels.PickedUp != null)//if parcel is picked up by drone
                    {
                        if (indexOfParcels.Delivered != null)//parcel is delivered
                            parcelAtCustomer.Status = ParcelStatus.delivery;
                        else
                            parcelAtCustomer.Status = ParcelStatus.pickup;
                    }
                    else
                        parcelAtCustomer.Status = ParcelStatus.schedul;
                }
                else
                    parcelAtCustomer.Status = ParcelStatus.creat;
                parcelAtCustomer.SenderOrRecepter = new CustomerInParcel();
                parcelAtCustomer.SenderOrRecepter.Id = blCustomer.Id;//Updates the source information of the parcel
                parcelAtCustomer.SenderOrRecepter.Name = blCustomer.Name;//Updates the source information of the parcel
            }
            return parcelAtCustomer;
        }
        private IEnumerable<ParcelInCustomer> parcelAtCustomers(bool flag, Customer blCustomer, List<DO.Parcel> parcelList)
        {
            return from item in parcelList
                   where flag ? item.SenderId == blCustomer.Id : item.TargetId == blCustomer.Id
                   select NewMethod(item, blCustomer);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomer(int customerId)
        {
            lock (dal)
                {
                    Customer blCustomer = new Customer();
                    try
                    {
                        DO.Customer dalCustomer = dal.GetCustomer(customerId);//finding customer using inputted id
                        dalCustomer.CopyPropertiesTo(blCustomer);//converting dal->bl
                        blCustomer.CustomerLocation = new();
                        blCustomer.CustomerLocation.Longitude = dalCustomer.Longitude;
                        blCustomer.CustomerLocation.Latitude = dalCustomer.Latitude;
                        blCustomer.ParcelsFromCustomers = new List<ParcelInCustomer>();
                        blCustomer.ParcelsToCustomers = new List<ParcelInCustomer>();
                        //goes through the parcels with the sent condition
                        List<DO.Parcel> parcelList = dal.GetParcels(parcel => parcel.SenderId == customerId || parcel.TargetId == customerId).ToList();
                        blCustomer.ParcelsFromCustomers = parcelAtCustomers(true, blCustomer, parcelList);
                        blCustomer.ParcelsToCustomers = parcelAtCustomers(false, blCustomer, parcelList);
                    }
                    catch (DO.DoesNotExistException ex)
                    {
                        throw new NotFoundInputException("ERROR\n", ex);
                    }
                    return blCustomer;
                }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<CustomerList> GetCustomers(Predicate<CustomerList> predicate = null)
        {
            lock (dal)
            {
                CustomerList customer = new();
                IEnumerable<CustomerList> Customers;//the customer list that we whant to returne
                Customers = from item in dal.GetCustomers()
                            select new CustomerList()
                            {
                                ParcelsSentAndDel = dal.GetParcels(x => x.SenderId == item.Id && x.Delivered != null).Count(),
                                ParcelsSentAndNotDel = dal.GetParcels(x => x.SenderId == item.Id && x.Delivered == null).Count(),
                                ParcelsResepted = dal.GetParcels(x => x.TargetId == item.Id && x.Delivered != null).Count(),
                                ParcelsOnTheWay = dal.GetParcels(x => x.TargetId == item.Id && x.PickedUp ==null).Count(),
                                Id = item.Id,
                                Name = item.Name,
                                Phone = item.Phone
                            };

                return Customers.Where(item => predicate == null ? true : predicate(item));
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(int id, string name, string phone)
        {
            lock (dal)
            {
                try
                {
                    dal.GetCustomer(id);//fineds the customer with the id
                    if (phone != "\n" && phone.Length < 10)//meens the phone is incurrect
                        throw new InvalidInputException($"The customer phone: {phone} is incorrect");
                    dal.UpdateCustomer(id, name, phone);//to update the customr
                }
                catch (DO.NotFoundInputException ex)
                {
                    throw new FailToUpdateException($"The customer: {id} wasn't found, the customer wasn't updated.\n", ex);
                }
                catch (InvalidInputException ex)
                {
                    throw new FailToUpdateException($"The phone: {phone} is incorrect, the customer: {id} wasn't updated.\n", ex);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public double DisSenderToResever(Customer sender, Customer resever)
        {
            return Distance.Haversine
            (sender.CustomerLocation.Longitude, sender.CustomerLocation.Latitude, resever.CustomerLocation.Longitude, resever.CustomerLocation.Latitude);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public DO.BaseStation FindMinDistanceOfCToBS(double latitude, double longitude)
        {
            lock (dal)
            {
                DO.BaseStation baseStation = new();
                double minDistance = 0;
                double distance = 0;
                //to go over all the station and check wich one is the closest
                foreach (var item in dal.GetBaseStations())
                {
                    //the distatce between the customer to the base station
                    distance = Distance.Haversine(item.Latitude, item.Longitude, latitude, longitude);
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteCustomer(int id)
        {
            lock (dal)
            {
                try
                {
                    dal.DeleteCustomer(id);
                }
                catch (DO.ItemIsDeletedException ex)
                {
                    throw new ItemIsDeletedException($"Customer: { id } is already deleted.");
                }
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool IsActive(int id)
        {
            if (dal.IsActive(id))
                return true;
            return false;
        }
    }
}

