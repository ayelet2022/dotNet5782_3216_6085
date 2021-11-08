using System;
using System.Collections.Generic;
using DAL;
using IDAL.DO;
namespace DalObject
{
    public class DalObject
    {
        public DalObject()
        {
            DataSource.Initialize();
        }
        /// <summary>
        /// adds a new base station to the arrey
        /// </summary>
        public  void AddBaseStation(BaseStation addBaseStation)
        {
            try
            {
                foreach (var itBS in DataSource.Stations)
                    if (addBaseStation.Id == itBS.Id)
                        //throw Exception. MyException("double");
                DataSource.Stations.Add(addBaseStation);
            }
            catch (Exception)
            {
                
            }
           
        }

        /// <summary>
        /// adds a new drone to the arrey
        /// </summary>
        public void AddDrone(Drone addDrone)
            {
               DataSource.Drones.Add(addDrone);
            }

        /// <summary>
        /// adds a new customer to the arrey
        /// </summary>
        /// <param name="newCustomer">the new customer that the user entered in main and needs to be added to the arrey</param>
        public void AddCustomer(Customer newCustomer)
            { 
                DataSource.Customers.Add(newCustomer);
            }

        /// <summary>
        /// adds a new parcel to the arrey
        /// </summary>
        /// <param name="newParcel">the new parce that the user entered in main and needs to be added to the arrey</param>
        public void AddParcel(Parcel newParcel)
            {
                newParcel.Id=DataSource.Config.RunningParcelId++;
                DataSource.Parcels.Add(newParcel);
            }

        /// <summary>
        ///  search for the base station in the arrey thet has the same id that the user enterd and returnes it
        /// </summary>
        /// <param name="idBaseStation"><the id(that was enterd by the user in main) of the basestation that the user wants to print/param>
        /// <returns>resturn the base station that needs to be printed</returns>
        public BaseStation PrintBaseStation(int idBaseStation)
            {
                int baseStationIndex = 0;
                while (DataSource.Stations[baseStationIndex].Id != idBaseStation)//search for the base station that has the same id has the id that the user enterd
                    baseStationIndex++;
                return DataSource.Stations[baseStationIndex];

            }

        /// <summary>
        ///  search for the drone in the arrey thet has the same id as the user enterd and returnes it
        /// </summary>
        /// <param name="idDrone">the id(that was enterd by the user in main) of the dron that the user wants to print</param>
        /// <returns>resturn the drone that needs to be printed</returns>
        public Drone PrintDrone(int idDrone)
        {
            int droneIndex = 0;
            while (DataSource.Drones[droneIndex].Id != idDrone)//search for the drone that has the same id has the id that the user enterd
                droneIndex++;
            return DataSource.Drones[droneIndex];
        }

        /// <summary>
        /// search for the customer in the arrey thet has the same id as the user enterd and retirnes it
        /// </summary>
        /// <param name="idCustomer">the id(that was enterd by the user in main) of the customer that the user wants to print</param>
        /// <returns>resturn the customer that needs to be printed</returns>
        public Customer PrintCustomer(int idCustomer)
            {
                int customerIndex = 0;
                while (DataSource.Customers[customerIndex].Id != idCustomer)//search for the customer that has the same id has the id that the user enterd
                    customerIndex++;
                return (DataSource.Customers[customerIndex]);

            }

        /// <summary>
        /// search for the parcel in the arrey thet has the same id as the user enterd and retirnes it
        /// </summary>
        /// <param name="idParcel">the id(that was enterd by the user in main) of the parcel that the user wants to print</param> 
        /// <returns>the parcel that needs to be printed</returns>
        public Parcel PrintParcel(int idParcel)
            {
                int parcelIndex = 0;
                while (DataSource.Parcels[parcelIndex].Id != idParcel)//search for the parcel that has the same id has the id that the user enterd
                    parcelIndex++;
                return (DataSource.Parcels[parcelIndex]);

            }

        /// <summary>
        /// search for the drone and the parcel that has the same id as what the user enterd 
        /// and changes the filed of the drone id inn the parcel detailes
        /// </summary>
        /// <param name="droneId">the drones id that the user enterd</param>
        /// <param name="parcelId">the parcel id that the user enterd</param>
        public void DronToAParcel(int droneId, int parcelId)
            {
                int pareclIndex = 0;
                while (DataSource.Parcels[pareclIndex].Id != parcelId)//search for the parcel that has the same id has the id that the user enterd
                    pareclIndex++;
                Parcel updateAParcel = DataSource.Parcels[pareclIndex];
                updateAParcel.DroneId = droneId;
                updateAParcel.Scheduled = DateTime.Now;
                updateAParcel.PickedUp = DateTime.Now;
                DataSource.Parcels[pareclIndex]=updateAParcel;
            }

        /// <summary>
        /// update the drone that has the same id as the user enterd to charge in the base station that has the id that the user enterd
        /// </summary>
        /// <param name="dronesId">the drones id that the user enterd</param>
        /// <param name="idOfBaseStation">the base station id that the user enterd</param>
        public void DronToCharger(int dronesId, int idOfBaseStation)
            {
                int droneIndex = 0;
                while (DataSource.Drones[droneIndex].Id != dronesId)//search for the drone that has the same id has the id that the user enterd
                    droneIndex++;
                int stationsIndex = 0;
                while (DataSource.Stations[stationsIndex].Id != idOfBaseStation)//search for the base station that has the same id has the id that the user enterd
                    stationsIndex++;
                DroneCharge updateADrone = new();
                updateADrone.DroneId = dronesId;
                updateADrone.StationId = idOfBaseStation;
                DataSource.DroneCharges.Add(updateADrone);
                BaseStation updateAStation = DataSource.Stations[stationsIndex];
                updateAStation.EmptyCharges--;
                DataSource.Stations[stationsIndex]= updateAStation;
            }

        /// <summary>
        /// free the drone-that has the same id that the user enterd from the charger that he was cherging from in the base station
        /// </summary>
        /// <param name="dronesId">the drones id that the user enterd</param>
        public void FreeDroneFromBaseStation(int dronesId)
            {
                int droneLocation = 0;
                while (DataSource.Drones[droneLocation].Id != dronesId)//search for the drone that has the same id has the id that the user enterd
                    droneLocation++;
                //DataSource.Drones[droneLocation].Battery = 100;
                //DataSource.Drones[droneLocation].Status = (DroneStatuses)0;//availabel
                int droneChargesIndex = 0;
                while (DataSource.DroneCharges[droneChargesIndex].DroneId != dronesId)//search for the drone charge that is chargine the drone that has the same id as the user enterd
                    droneChargesIndex++;
                DataSource.DroneCharges.Remove(DataSource.DroneCharges[droneChargesIndex]);
                int stationLocation = 0;
                while (DataSource.Stations[stationLocation].Id != DataSource.DroneCharges[droneChargesIndex].StationId)//search for the base station that has the same id as the one in the chrger
                    stationLocation++;
                BaseStation updateAStation = DataSource.Stations[stationLocation];
                updateAStation.EmptyCharges--;
                DataSource.Stations[stationLocation] = updateAStation;
            }

        /// <summary>
        /// updates the time that the parcel was picked up by the dron
        /// </summary>
        /// <param name="newId">the id pf the parcel that was enterd by the user</param>
        public void PickUpParcel(int newId)
            {
                int parcelsIndex = 0;
                while (DataSource.Parcels[parcelsIndex].Id != newId)//search for the parcel that has the same id has the id that the user enterd
                    parcelsIndex++;
                Parcel updateAParcel= DataSource.Parcels[parcelsIndex];
                updateAParcel.PickedUp = DateTime.Now;
                DataSource.Parcels[parcelsIndex]= updateAParcel;
            }

        /// <summary>
        /// updates the time that the parcel was deleverd to the customer
        /// </summary>
        /// <param name="newId">the id pf the parcel that was enterd by the user</param>
        public void ParcelToCustomer(int newId)
        {
                int parcelsIndex = 0;
                while (DataSource.Parcels[parcelsIndex].Id != newId)//search for the parcel that has the same id has the id that the user enterd
                    parcelsIndex++;
                Parcel updateAParcel = DataSource.Parcels[parcelsIndex];
                updateAParcel.Delivered = DateTime.Now;
                updateAParcel.DroneId = 0;
                DataSource.Parcels[parcelsIndex]= updateAParcel;
        }

        /// <summary>
        /// copyes the values of al the base stations in order to print them
        /// </summary>
        /// <returns>the new arrey that has the the base stations</returns>
        public List<BaseStation> PrintBaseStations()
        {
            return DataSource.Stations;
        }

        /// <summary>
        /// copyes the values of all the drones in order to print them
        /// </summary>
        /// <returns>the new arrey that has the the drones</returns>
        public List<Drone> PrintDrones()
        {
            return DataSource.Drones;
        }

        /// <summary>
        /// copyes the values of all the customer in order to print them
        /// </summary>
        /// <returns>the new arrey that has the the customers</returns>
        public List<Customer> PrintCustomers()
        {
            return DataSource.Customers;
        }

        /// <summary>
        /// copyes the values of all the parcel in order to print them
        /// </summary>
        /// <returns>the new arrey that has the the parceles</returns>
        public List<Parcel> PrintPercels()
        {
            return DataSource.Parcels;
        }

        /// <summary>
        /// search for all the drones that are available and copy them to a new arrey  
        /// </summary>
        /// <returns>the new arrey that has all the drones that are availeble</returns>
        public List<Parcel>  ParcelThatWerenNotPaired()
            {
                List<Parcel>  Parcels = new ();
                foreach (var itBS in DataSource.Parcels)
                {
                    if (itBS.DroneId == 0)
                        Parcels.Add(itBS);
                }
                return Parcels;
            }

        /// <summary>
        /// search for tha base station that has available charges and copys them to a new arrey
        /// </summary>
        /// <returns>the new arrey that has all the base station that has free charges</returns>
        public List<BaseStation> BaseStationWithAvailableCharges()
            {
                List<BaseStation> Stations = new();
                foreach (var itBS in DataSource.Stations)
                {
                    if (itBS.EmptyCharges > 0)
                        Stations.Add(itBS);
                }
                return Stations;
            }

        public double[] AskForBattery()
        {
            return;
        }

    }
}

     



