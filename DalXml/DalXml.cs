using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DalApi;
using DO;

namespace Dal
{
    internal partial class DalXml : IDal
    {
        private static string DroneXml = @"Drone.xml";
        private static string StationXml = @"Station.xml";
        private static string ParcelXml = @"Parcel.xml";
        private static string CustomerXml = @"Customer.xml";
        private static string DroneChargeXml = @"DroneCharge.xml";
        private static string ConfigXml = @"Config.xml";

        internal static DalXml Instance { get; set; } = new DalXml();
        static DalXml() { //XMLTools.SaveListToXMLSerializer(new List<int> { 1010 },"ConfigXml.xml");
                          }
        private DalXml()
        {
            //DataSource.Initialize();
        }
        public double[] AskForBattery()
        {
            XElement configXml = XMLTools.LoadListFromXMLElement(ConfigXml);
            double[] arr = { 
                double.Parse(configXml.Element("Available").Value),
                double.Parse(configXml.Element("Light").Value),
                double.Parse(configXml.Element("MediumWeight").Value),
                double.Parse(configXml.Element("Heavy").Value),
                double.Parse(configXml.Element("ChargingRate").Value) };
            return arr;
        }

        #region DRONE
        public IEnumerable<Drone> GetDrones(Predicate<Drone> predicate = null)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            return from item in drones
                   where predicate == null ? true : predicate(item) && item.IsActive
                   select item;
        }
        public void UpdateDrone(int id, string newModel)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            int droneIndex = drones.FindIndex(item => item.Id == id && item.IsActive);
            //checks if the drone was found
            if (droneIndex == -1)
                throw new DoesNotExistException($"Drone id: {id} does not exist.");
            Drone drone = drones[droneIndex];
            drone.Model = newModel;
            drones[droneIndex] = drone;//to change the drone in the drone list
            XMLTools.SaveListToXMLSerializer(drones, DroneXml);
        }
        public Drone GetDrone(int idDrone)
        {
            try
            {
                List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
                //search for the drone that has the same id has the id that the user enterd
                return drones.First(item => item.Id == idDrone && item.IsActive);
            }
            catch (InvalidOperationException ex)
            {
                throw new DoesNotExistException($"Drone id: {idDrone} does not exist.");
            }
        }
        public void AddDrone(Drone addDrone)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            if (drones.Exists(item => item.Id == addDrone.Id && item.IsActive))
                throw new ExistsException($"Drone id: {addDrone.Id} already exists.");
            addDrone.IsActive = true;
            drones.Add(addDrone);
            XMLTools.SaveListToXMLSerializer(drones, DroneXml);
        }
        public void DronToCharger(int dronesId, int idOfBaseStation)
        {
            List<BaseStation> stations = XMLTools.LoadListFromXMLSerializer<BaseStation>(StationXml);
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            //search for the drone that has the same id has the id that the user enterd
            int droneIndex = drones.FindIndex(item => item.Id == dronesId && item.IsActive);
            if (droneIndex == -1)
                throw new DoesNotExistException($"Drone id: {dronesId} does not exist.");
            //search for the base station that has the same id has the id that the user enterd
            int baseStationIndex = stations.FindIndex(item => item.Id == idOfBaseStation);
            if (baseStationIndex == -1)
                throw new DoesNotExistException($"Base station id: {idOfBaseStation} does not exists.");
            DroneCharge droneCharge = new();
            droneCharge.StartCharging = DateTime.Now;
            droneCharge.DroneId = dronesId;
            droneCharge.StationId = idOfBaseStation;
            AddDroneCharge(droneCharge);
            BaseStation updateAStation = stations[baseStationIndex];
            updateAStation.EmptyCharges--;
            stations[baseStationIndex] = updateAStation;
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }
        public void FreeDroneFromBaseStation(int dronesId)
        {
            List<BaseStation> stations = XMLTools.LoadListFromXMLSerializer<BaseStation>(StationXml);
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            List<DroneCharge> droneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
            //search for the drone that has the same id has the id that the user enterd
            int droneIndex = drones.FindIndex(item => item.Id == dronesId && item.IsActive);
            if (droneIndex == -1)
                throw new DoesNotExistException($"Drone id: {dronesId} does not exist.");
            int droneChargesIndex = droneCharges.FindIndex(item => item.DroneId == dronesId);
            if (droneChargesIndex == -1)
                throw new NotFoundInputException($"Drone id: {dronesId} isn't charging.");
            int baseStationIndex = stations.FindIndex(item => item.Id == droneCharges[droneChargesIndex].StationId);
            if (baseStationIndex == -1)
                throw new DoesNotExistException($"Base station id: {baseStationIndex} does not exists.");
            BaseStation updateAStation = stations[baseStationIndex];
            updateAStation.EmptyCharges++;
            stations[baseStationIndex] = updateAStation;
            droneCharges.Remove(droneCharges[droneChargesIndex]);
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
            XMLTools.SaveListToXMLSerializer(droneCharges, DroneChargeXml);
        }
        public void DeleteDrone(int id)
        {
            try
            {
                List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
                Drone drone = GetDrone(id);
                drones.Remove(drone);
                XMLTools.SaveListToXMLSerializer(drones, DroneXml);
            }
            catch (DoesNotExistException ex)
            {
                throw new ItemIsDeletedException($"Drone: { id } is already deleted.");
            }
        }
        public void NotActiveDrone(int id)
        {
            try
            {
                List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
                int droneIndex = drones.FindIndex(item => item.Id == id);
                Drone drone = GetDrone(id);
                drone.IsActive = false;
                drones[droneIndex] = drone;
                XMLTools.SaveListToXMLSerializer(drones, DroneXml);
            }
            catch (DoesNotExistException ex)
            {
                throw new ItemIsDeletedException($"Drone: { id } is already deleted.");
            }
        }
        #endregion

        #region STATION
        public void AddBaseStation(BaseStation addBaseStation)
        {
            List<BaseStation> stations = XMLTools.LoadListFromXMLSerializer<BaseStation>(StationXml);
            if (stations.Exists(item => item.Id == addBaseStation.Id && item.IsActive))
                throw new ExistsException($"Base station id: {addBaseStation.Id} already exists.");
            addBaseStation.IsActive = true;
            stations.Add(addBaseStation);
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }
        public IEnumerable<BaseStation> GetBaseStations(Predicate<BaseStation> predicate = null)
        {
            List<BaseStation> stations = XMLTools.LoadListFromXMLSerializer<BaseStation>(StationXml);
            return from item in stations
                   where (predicate == null ? true : predicate(item)) && item.IsActive
                   select item;
        }
        public BaseStation GetBaseStation(int idBaseStation)
        {
            try
            {
                List<BaseStation> stations = XMLTools.LoadListFromXMLSerializer<BaseStation>(StationXml);
                //search for the base station that has the same id has the id that the user enterd
                return stations.First(item => item.Id == idBaseStation && item.IsActive);
            }
            catch (InvalidOperationException ex)
            {
                throw new DoesNotExistException($"Base station id: {idBaseStation} does not exists.");
            }
        }
        public void UpdateStation(int id, string newName, int charges)
        {
            List<BaseStation> stations = XMLTools.LoadListFromXMLSerializer<BaseStation>(StationXml);
            XElement droneCharge = XMLTools.LoadListFromXMLElement(DroneChargeXml);
            List<DroneCharge> droneCharges = (List<DroneCharge>)droneCharge.Elements(DroneChargeXml);
            int baseStationIndex = stations.FindIndex(item => item.Id == id && item.IsActive);
            BaseStation baseStation = stations[baseStationIndex];
            if (newName != "\n")
                baseStation.Name = newName;
            int sum = droneCharges.Where(item => item.StationId == id).Count();
            baseStation.EmptyCharges = charges - sum;
            stations[baseStationIndex] = baseStation;//to update the station in the list of base stations
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }
        public void DeleteBaseStation(int id)
        {
            try
            {
                List<BaseStation> stations = XMLTools.LoadListFromXMLSerializer<BaseStation>(StationXml);
                int baseStationIndex = stations.FindIndex(item => item.Id == id && item.IsActive);
                BaseStation baseStation = GetBaseStation(id);
                baseStation.IsActive = false;
                int stationIndex = DataSource.Stations.FindIndex(item => item.Id == id);
                stations[stationIndex] = baseStation;
                XMLTools.SaveListToXMLSerializer(stations, StationXml);
            }
            catch (DoesNotExistException ex)
            {
                throw new ItemIsDeletedException($"Station: { id } is already deleted.");
            }
        }

        #endregion

        #region PARCEL
        public void AddParcel(Parcel newParcel)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            XElement configXml = XMLTools.LoadListFromXMLElement(ConfigXml);
            newParcel.Id = 1 + int.Parse(configXml.Element("RunningParcelId").Value);
            newParcel.CreatParcel = DateTime.Now;
            newParcel.Delivered = null;
            newParcel.PickedUp = null;
            newParcel.Scheduled = null;
            newParcel.IsActive = true;
            parcels.Add(newParcel);
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
            configXml.Element("RunningParcelId").Value = newParcel.Id.ToString();
            XMLTools.SaveListToXMLElement(configXml, "Config.xml");
        }
        public void DeleteParcel(int id)
        {
            try
            {
                List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
                int parcelIndex = parcels.FindIndex(item => item.Id == id);
                Parcel parcel = GetParcel(id);
                if (parcel.Scheduled == null)
                    parcels.Remove(parcel);
                else
                {
                    parcel.IsActive = false;
                    parcels[parcelIndex] = parcel;
                }
                XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
            }
            catch (DoesNotExistException ex)
            {
                throw new ItemIsDeletedException($"Parcel: { id } is already deleted.");
            }
        }
        public Parcel GetParcel(int idParcel)
        {
            try
            {
                List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
                //search for the parcel that has the same id has the id that the user enterd
                Parcel parcel = parcels.First(item => item.Id == idParcel);
                if (parcel.IsActive)
                    return parcel;
                else
                    throw new ItemIsDeletedException();
            }
            catch (InvalidOperationException ex)
            {
                throw new DoesNotExistException($"Parcel id: { idParcel } does not exists.");
            }
            catch (ItemIsDeletedException ex)
            {
                throw new DoesNotExistException($"Parcel id: { idParcel } does not exists.");
            }
        }
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> predicate = null)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            return from item in parcels
                   where predicate == null ? true : predicate(item)
                   where item.IsActive == true
                   select item;
        }
        public void DronToAParcel(int droneId, int parcelId)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            //search for the drone that has the same id has the id that the user enterd
            int droneIndex = drones.FindIndex(item => item.Id == droneId);
            if (droneIndex == -1)
                throw new DoesNotExistException($"Drone id: {droneId} does not exist.");
            //search for the parcel that has the same id has the id that the user enterd
            int parcelIndex = parcels.FindIndex(item => item.Id == parcelId && item.IsActive);
            if (parcelIndex == -1)
                throw new DoesNotExistException($"Parcel id: { parcelId } does not exists.");
            Parcel updateAParcel = parcels[parcelIndex];
            updateAParcel.DroneId = droneId;
            updateAParcel.Scheduled = DateTime.Now;
            parcels[parcelIndex] = updateAParcel;
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
        }
        public void PickUpParcel(int newId)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            //search for the parcel that has the same id has the id that the user enterd
            int parcelIndex = parcels.FindIndex(item => item.Id == newId && item.IsActive);
            if (parcelIndex == -1)
                throw new DoesNotExistException($"Parcel id: { newId } does not exists.");
            Parcel updateAParcel = parcels[parcelIndex];
            updateAParcel.PickedUp = DateTime.Now;
            parcels[parcelIndex] = updateAParcel;
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
        }
        public void ParcelToCustomer(int newId)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            //search for the parcel that has the same id has the id that the user enterd
            int parcelIndex = parcels.FindIndex(item => item.Id == newId && item.IsActive);
            if (parcelIndex == -1)
                throw new DoesNotExistException($"Parcel id: { newId } does not exists.");
            Parcel updateAParcel = parcels[parcelIndex];
            updateAParcel.Delivered = DateTime.Now;
            updateAParcel.DroneId = 0;
            parcels[parcelIndex] = updateAParcel;
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
        }
        #endregion

        #region CUSTOMER
        public void AddCustomer(Customer newCustomer)
        {
            XElement customerXml = XMLTools.LoadListFromXMLElement(CustomerXml);

            XElement customer = (from cus in customerXml.Elements()
                                 where cus.Element("Id").Value == newCustomer.Id.ToString() && cus.Element("IsActive").Value == "true"
                                 select cus).FirstOrDefault();
            if (customer != null)
            {
                throw new ExistsException($"Customer id: {newCustomer.Id} already exists.");
            }

            XElement CustomerElem = new XElement("Customer",
                                 new XElement("Id", newCustomer.Id),
                                 new XElement("Name", newCustomer.Name),
                                 new XElement("Phone", newCustomer.Phone),
                                 new XElement("Longitude", newCustomer.Longitude),
                                 new XElement("Latitude", newCustomer.Latitude),
                                 new XElement("IsActive", true));

            customerXml.Add(CustomerElem);
            XMLTools.SaveListToXMLElement(customerXml, CustomerXml);
        }
        public Customer GetCustomer(int idCustomer)
        {
            XElement customerXml = XMLTools.LoadListFromXMLElement(CustomerXml);

            Customer customer = (from cus in customerXml.Elements()
                                 where cus.Element("Id").Value == idCustomer.ToString() && cus.Element("IsActive").Value =="true"
                                 select new Customer()
                                 {
                                     Id = int.Parse(cus.Element("Id").Value),
                                     Name = cus.Element("Name").Value,
                                     Phone = cus.Element("Phone").Value,
                                     Longitude = double.Parse(cus.Element("Longitude").Value),
                                     Latitude = double.Parse(cus.Element("Latitude").Value),
                                     IsActive=bool.Parse(cus.Element("IsActive").Value)
                                 }
                        ).FirstOrDefault();

            if (customer.Id != 0)
            {
                return customer;
            }
            else
            {
                throw new DoesNotExistException($"Customer id: { idCustomer } does not exists.");
            }
        }
        public void UpdateCustomer(int id, string name, string phone)
        {
            XElement customerXml = XMLTools.LoadListFromXMLElement(CustomerXml);

            XElement customer = (from cus in customerXml.Elements()
                                 where cus.Element("Id").Value == id.ToString() && cus.Element("IsActive").Value == "true"
                                 select cus).FirstOrDefault();
            if (customer == null)
                throw new DoesNotExistException($"Customer id: {id} does not exist.");

            customer.Element("Id").Value = id.ToString();
            customer.Element("Name").Value = name;
            customer.Element("Phone").Value = phone;
            customer.Element("IsActive").Value = true.ToString();
            XMLTools.SaveListToXMLElement(customerXml, CustomerXml);
        }
        public IEnumerable<Customer> GetCustomers(Predicate<Customer> predicate = null)
        {
            XElement customerXml = XMLTools.LoadListFromXMLElement(CustomerXml);
            IEnumerable<Customer> customer = from cus in customerXml.Elements()
                                             where  cus.Element("IsActive").Value == "true"
                                             select new Customer()
                                             {
                                                 Id = int.Parse(cus.Element("Id").Value),
                                                 Name = cus.Element("Name").Value,
                                                 Phone = cus.Element("Phone").Value,
                                                 Longitude = double.Parse(cus.Element("Longitude").Value),
                                                 Latitude = double.Parse(cus.Element("Latitude").Value),
                                                 IsActive = bool.Parse(cus.Element("IsActive").Value)
                                             };
            return customer.Where(item=>predicate==null?true:predicate(item)).Select(item => item);
        }
        public void DeleteCustomer(int id)
        {

            XElement customerXml = XMLTools.LoadListFromXMLElement(CustomerXml);

            XElement customer = (from cus in customerXml.Elements()
                                 where cus.Element("Id").Value == id.ToString() && cus.Element("IsActive").Value == "true"
                                 select cus).FirstOrDefault();
            if (customer == null)
                throw new ItemIsDeletedException($"Drone: { id } is already deleted.");
            customer.Element("Id").Value = id.ToString();
            customer.Element("IsActive").Value = "false";
            XMLTools.SaveListToXMLElement(customerXml, CustomerXml);
        }
        #endregion

        #region DRONE_CHARGE
        public IEnumerable<DroneCharge> GetDroneCharges(Predicate<DroneCharge> predicate = null)
        {
            List<DroneCharge> droneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
            return from item in droneCharges
                   where predicate == null ? true : predicate(item)
                   select item;
        }
        public DroneCharge GetDroneCharge(int droneId)
        {
            try
            {
                List<DroneCharge> droneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
                return droneCharges.First(x => x.DroneId == droneId);

            }
            catch (Exception)
            {
                throw new DoesNotExistException($"Drone id: {droneId} does not exist.");
            }
        }
        public void AddDroneCharge(DroneCharge droneCharge)
        {
            List<DroneCharge> droneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
            //chcks if the charger already exists
            if (droneCharges.Exists(item => item.DroneId == droneCharge.DroneId && item.StationId == droneCharge.StationId))
                throw new ExistsException($"The Drone:{droneCharge.DroneId} already charging.");
            droneCharges.Add(droneCharge);
            XMLTools.SaveListToXMLSerializer(droneCharges, DroneChargeXml);
        }
        #endregion
    }
}

