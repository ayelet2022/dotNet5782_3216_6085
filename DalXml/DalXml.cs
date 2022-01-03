using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using DalApi;
using DO;

namespace DalXml
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
            DataSource.Initialize();
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
            throw new NotImplementedException();
        }

        public void AddParcel(Parcel newParcel)
        {
            throw new NotImplementedException();
        }

        public void AddDroneCharge(DroneCharge droneCharge)
        {
            throw new NotImplementedException();
        }

        public BaseStation GetBaseStation(int idBaseStation)
        {
            throw new NotImplementedException();
        }

        public Drone GetDrone(int idDrone)
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomer(int idCustomer)
        {
            throw new NotImplementedException();
        }

        public Parcel GetParcel(int idParcel)
        {
            throw new NotImplementedException();
        }

        public void DronToAParcel(int droneId, int parcelId)
        {
            throw new NotImplementedException();
        }

        public void DronToCharger(int dronesId, int idOfBaseStation)
        {
            throw new NotImplementedException();
        }

        public void FreeDroneFromBaseStation(int dronesId)
        {
            throw new NotImplementedException();
        }

        public void PickUpParcel(int newId)
        {
            throw new NotImplementedException();
        }

        public void ParcelToCustomer(int newId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BaseStation> GetBaseStations(Predicate<BaseStation> predicate = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Drone> GetDrones(Predicate<Drone> predicate = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetCustomers(Predicate<Customer> predicate = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> predicate = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DroneCharge> GetDroneCharges(Predicate<DroneCharge> predicate = null)
        {
            throw new NotImplementedException();
        }

        public void UpdateCustomer(int id, string name, string phone)
        {
            throw new NotImplementedException();
        }

        public void UpdateDrone(int id, string newModel)
        {
            throw new NotImplementedException();
        }

        public void UpdateStation(int id, string newName, int emptyCharges)
        {
            throw new NotImplementedException();
        }

        public DroneCharge GetDroneCharge(int droneId)
        {
            throw new NotImplementedException();
        }

        public void DeletParcel(int id)
        {
            throw new NotImplementedException();
        }
    }
}

