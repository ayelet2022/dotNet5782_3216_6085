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
        private static string DroneXml = @"DroneXml.xml";
        private static string StationXml = @"StationXml.xml";
        private static string ParcelXml = @"ParcelXml.xml";
        private static string CustomerXml = @"CustomerXml.xml";
        private static string DroneChargeXml = @"DroneChargeXml.xml";
        internal static DalXml Instance { get; set; } = new DalXml();
        static DalXml() { }
        private DalXml()
        {
            //DataSource.Initialize();
        }
        public double[] AskForBattery()
        {
            double[] arr = { DataSource.Config.Available, DataSource.Config.Light, DataSource.Config.MediumWeight, DataSource.Config.Heavy, DataSource.Config.ChargingRate };
            return arr;
        }

        public void AddBaseStation(BaseStation addBaseStation)
        {
            List<BaseStation> stations = XMLTools.LoadListFromXMLSerializer<BaseStation>(StationXml);
            if (stations.Exists(item => item.Id == addBaseStation.Id))
                throw new ExistsException($"Base station id: {addBaseStation.Id} already exists.");
            stations.Add(addBaseStation);
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }

        public void AddDrone(Drone addDrone)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            if (drones.Exists(item => item.Id == addDrone.Id))
                throw new ExistsException($"Drone id: {addDrone.Id} already exists.");
            drones.Add(addDrone);
            XMLTools.SaveListToXMLSerializer(drones, DroneXml);
        }

        public void AddCustomer(Customer newCustomer)
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);
            if (customers.Exists(item => item.Id == newCustomer.Id))
                throw new ExistsException($"Customer id: {newCustomer.Id} already exists.");
            customers.Add(newCustomer);
            XMLTools.SaveListToXMLSerializer(customers, CustomerXml);
        }

        public void AddParcel(Parcel newParcel)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            newParcel.Id = DataSource.Config.RunningParcelId++;
            newParcel.CreatParcel = DateTime.Now;
            newParcel.Delivered = null;
            newParcel.PickedUp = null;
            newParcel.Scheduled = null;
            parcels.Add(newParcel);
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
        }

        public void AddDroneCharge(DroneCharge droneCharge)
        {
            XElement droneChargeRoot = XMLTools.LoadListFromXMLElement(DroneChargeXml);
            XElement droneId = new XElement("DroneId", droneCharge.DroneId);
            XElement stationId = new XElement("StationId", droneCharge.StationId);
            XElement startCharging = new XElement("StartCharging", droneCharge.StartCharging);
            droneChargeRoot.Add(new XElement("DroneCharge", droneId, stationId, startCharging));
            droneChargeRoot.Save(DroneChargeXml);
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

        public Customer GetCustomer(int idCustomer)
        {
            try
            {
                List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);
                //search for the customer that has the same id has the id that the user enterd
                return customers.First(item => item.Id == idCustomer);
            }
            catch (InvalidOperationException ex)
            {
                throw new DoesNotExistException($"Customer id: { idCustomer } does not exists.");
            }
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
            XElement droneCharge = XMLTools.LoadListFromXMLElement(DroneChargeXml);
            List<DroneCharge> droneCharges = (List<DroneCharge>)droneCharge.Elements(DroneChargeXml);
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
            XMLTools.SaveListToXMLElement(droneCharge, DroneChargeXml);
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

        public IEnumerable<BaseStation> GetBaseStations(Predicate<BaseStation> predicate = null)
        {
            List<BaseStation> stations = XMLTools.LoadListFromXMLSerializer<BaseStation>(StationXml);
            return from item in stations
                   where predicate == null ? true : predicate(item)
                   select item;
        }

        public IEnumerable<Drone> GetDrones(Predicate<Drone> predicate = null)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            return from item in drones
                   where predicate == null ? true : predicate(item)
                   select item;
        }

        public IEnumerable<Customer> GetCustomers(Predicate<Customer> predicate = null)
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);
            return from item in customers
                   where predicate == null ? true : predicate(item)
                   select item;
        }

        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> predicate = null)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            return from item in parcels
                   where predicate == null ? true : predicate(item)
                   select item;
        }

        public IEnumerable<DroneCharge> GetDroneCharges(Predicate<DroneCharge> predicate = null)
        {
            XElement droneChargeRoot = XMLTools.LoadListFromXMLElement(DroneChargeXml);
            IEnumerable<DroneCharge> droneCharges;
            try
            {
                droneCharges = (from p in droneChargeRoot.Elements()
                                select new DroneCharge()
                                {
                                    DroneId = Convert.ToInt32(p.Element("DroneId").Value),
                                    StationId = Convert.ToInt32(p.Element("StationId").Value),
                                    StartCharging = Convert.ToDateTime(p.Element("StartCharging").Value),
                                });
            }
            catch
            {
                droneCharges = null;
            }
            return droneCharges;
        }

        public void UpdateCustomer(int id, string name, string phone)
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);
            int customerIndex = customers.FindIndex(item => item.Id == id);
            Customer customer = customers[customerIndex];
            //meens we want to update the name 
            if (name != "\n")
                customer.Name = name;
            //meens we want to update the phone
            if (phone != "\n")
                customer.Phone = phone;
            customers[customerIndex] = customer;//to change the customer in the list of customers
            XMLTools.SaveListToXMLSerializer(customers, CustomerXml);
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

        public DroneCharge GetDroneCharge(int droneId)
        {
            XElement droneChargeRoot = XMLTools.LoadListFromXMLElement(DroneChargeXml);
            DroneCharge droneCharge;
            try
            {
                droneCharge = (from p in droneChargeRoot.Elements()
                               where Convert.ToInt32(p.Element("DroneId").Value) == droneId
                               select new DroneCharge()
                               {
                                   DroneId = Convert.ToInt32(p.Element("DroneId").Value),
                                   StationId = Convert.ToInt32(p.Element("StationId").Value),
                                   StartCharging = Convert.ToDateTime(p.Element("StartCharging").Value),
                               }).FirstOrDefault();
            }
            catch
            {
                droneCharge = default;
            }
            return droneCharge;
        }

        public void DeletParcel(int id)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            parcels.Remove(GetParcel(id));
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
        }
    }
}

