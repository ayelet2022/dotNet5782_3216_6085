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
                   where predicate == null ? true : predicate(item)
                   select item;
        }
        public void UpdateDrone(int id, string newModel)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            int droneIndex = drones.FindIndex(item => item.Id == id);
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
                return drones.First(item => item.Id == idDrone);
            }
            catch (InvalidOperationException ex)
            {
                throw new DoesNotExistException($"Drone id: {idDrone} does not exist.");
            }
        }
        public void AddDrone(Drone addDrone)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            if (drones.Exists(item => item.Id == addDrone.Id))
                throw new ExistsException($"Drone id: {addDrone.Id} already exists.");
            drones.Add(addDrone);
            XMLTools.SaveListToXMLSerializer(drones, DroneXml);
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
            int parcelIndex = parcels.FindIndex(item => item.Id == parcelId);
            if (parcelIndex == -1)
                throw new DoesNotExistException($"Parcel id: { parcelId } does not exists.");
            Parcel updateAParcel = parcels[parcelIndex];
            updateAParcel.DroneId = droneId;
            updateAParcel.Scheduled = DateTime.Now;
            parcels[parcelIndex] = updateAParcel;
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
        }

        public void DronToCharger(int dronesId, int idOfBaseStation)
        {
            List<BaseStation> stations = XMLTools.LoadListFromXMLSerializer<BaseStation>(StationXml);
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            //search for the drone that has the same id has the id that the user enterd
            int droneIndex = drones.FindIndex(item => item.Id == dronesId);
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
            int droneIndex = drones.FindIndex(item => item.Id == dronesId);
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

        public void PickUpParcel(int newId)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            //search for the parcel that has the same id has the id that the user enterd
            int parcelIndex = parcels.FindIndex(item => item.Id == newId);
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
            int parcelIndex = parcels.FindIndex(item => item.Id == newId);
            if (parcelIndex == -1)
                throw new DoesNotExistException($"Parcel id: { newId } does not exists.");
            Parcel updateAParcel = parcels[parcelIndex];
            updateAParcel.Delivered = DateTime.Now;
            updateAParcel.DroneId = 0;
            parcels[parcelIndex] = updateAParcel;
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
        }
        #endregion

        #region STATION
        public void AddBaseStation(BaseStation addBaseStation)
        {
            List<BaseStation> stations = XMLTools.LoadListFromXMLSerializer<BaseStation>(StationXml);
            if (stations.Exists(item => item.Id == addBaseStation.Id))
                throw new ExistsException($"Base station id: {addBaseStation.Id} already exists.");
            stations.Add(addBaseStation);
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }
        public IEnumerable<BaseStation> GetBaseStations(Predicate<BaseStation> predicate = null)
        {
            List<BaseStation> stations = XMLTools.LoadListFromXMLSerializer<BaseStation>(StationXml);
            return from item in stations
                   where predicate == null ? true : predicate(item)
                   select item;
        }

        public BaseStation GetBaseStation(int idBaseStation)
        {
            try
            {
                List<BaseStation> stations = XMLTools.LoadListFromXMLSerializer<BaseStation>(StationXml);
                //search for the base station that has the same id has the id that the user enterd
                return stations.First(item => item.Id == idBaseStation);
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
            int baseStationIndex = stations.FindIndex(item => item.Id == id);
            BaseStation baseStation = stations[baseStationIndex];
            if (newName != "\n")
                baseStation.Name = newName;
            int sum = droneCharges.Where(item => item.StationId == id).Count();
            baseStation.EmptyCharges = charges - sum;
            stations[baseStationIndex] = baseStation;//to update the station in the list of base stations
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }

        public void DroneInStation(int stationId)
        {
            List<BaseStation> stations = XMLTools.LoadListFromXMLSerializer<BaseStation>(StationXml);
            int baseStationIndex =stations.FindIndex(item => item.Id == stationId);
            BaseStation baseStation = stations[baseStationIndex];
            baseStation.EmptyCharges--;
            stations[baseStationIndex] = baseStation;
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
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
            parcels.Add(newParcel);
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
            configXml.Element("RunningParcelId").Value = newParcel.Id.ToString();
            XMLTools.SaveListToXMLElement(configXml, "ConfigXml.xml");
        }
        public void DeletParcel(int id)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            parcels.Remove(GetParcel(id));
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
        }
        public Parcel GetParcel(int idParcel)
        {
            try
            {
                List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
                //search for the parcel that has the same id has the id that the user enterd
                return parcels.First(item => item.Id == idParcel);
            }
            catch (InvalidOperationException ex)
            {
                throw new DoesNotExistException($"Parcel id: { idParcel } does not exists.");
            }
        }
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> predicate = null)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            return from item in parcels
                   where predicate == null ? true : predicate(item)
                   select item;
        }
        #endregion

        #region CUSTOMER
        public void AddCustomer(Customer newCustomer)
        {
            XElement customerXml = XMLTools.LoadListFromXMLElement(CustomerXml);

            XElement customer = (from cus in customerXml.Elements()
                                 where cus.Element("Id").Value == newCustomer.Id.ToString()
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
                                 new XElement("Latitude", newCustomer.Latitude));

            customerXml.Add(CustomerElem);
            XMLTools.SaveListToXMLElement(customerXml, CustomerXml);
        }
        public Customer GetCustomer(int idCustomer)
        {
            XElement customerXml = XMLTools.LoadListFromXMLElement(CustomerXml);

            Customer customer = (from cus in customerXml.Elements()
                                 where cus.Element("Id").Value == idCustomer.ToString()
                                 select new Customer()
                                 {
                                     Id = int.Parse(cus.Element("Id").Value),
                                     Name = cus.Element("Name").Value,
                                     Phone = cus.Element("Phone").Value,
                                     Longitude = double.Parse(cus.Element("Longitude").Value),
                                     Latitude = double.Parse(cus.Element("Latitude").Value)
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
                                 where cus.Element("Id").Value == id.ToString()
                                 select cus).FirstOrDefault();
            if (customer == null)
                throw new DoesNotExistException($"Customer id: {id} does not exist.");

            customer.Element("Id").Value = id.ToString();
            customer.Element("Name").Value = name;
            customer.Element("Phone").Value = phone;
            XMLTools.SaveListToXMLElement(customerXml, CustomerXml);
        }
        public IEnumerable<Customer> GetCustomers(Predicate<Customer> predicate = null)
        {
            XElement customerXml = XMLTools.LoadListFromXMLElement(CustomerXml);
            IEnumerable<Customer> customer = from cus in customerXml.Elements()
                                             select new Customer()
                                             {
                                                 Id = int.Parse(cus.Element("Id").Value),
                                                 Name = cus.Element("Name").Value,
                                                 Phone = cus.Element("Phone").Value,
                                                 Longitude = double.Parse(cus.Element("Longitude").Value),
                                                 Latitude = double.Parse(cus.Element("Latitude").Value)
                                             };
            return customer.Select(item => item);
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

