using System;
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
        public static void AddBaseStation(BaseStation addBaseStation)
            {
            DataSource.Stations[DataSource.Config.StationsIndex++] = addBaseStation;
            }

        /// <summary>
        /// adds a new drone to the arrey
        /// </summary>
        public static void AddDrone(Drone addDrone)
            {
               DataSource.Drones[DataSource.Config.DronesIndex++]=addDrone;
            }

        /// <summary>
        /// adds a new customer to the arrey
        /// </summary>
        /// <param name="newCustomer">the new customer that the user entered in main and needs to be added to the arrey</param>
        public static void AddCustomer(Customer newCustomer)
            {
                DataSource.Customers[DataSource.Config.CustomersIndex++]= newCustomer;
            }

        /// <summary>
        /// adds a new parcel to the arrey
        /// </summary>
        /// <param name="newParcel">the new parce that the user entered in main and needs to be added to the arrey</param>
        public static void AddParcel(Parcel newParcel)
            {
                DataSource.Parcels[DataSource.Config.ParcelsIndex] = newParcel;
                DataSource.Parcels[DataSource.Config.ParcelsIndex].Id = DataSource.Config.RunningParcelId++;
                DataSource.Config.ParcelsIndex++;
            }

        /// <summary>
        ///  search for the base station in the arrey thet has the same id that the user enterd and returnes it
        /// </summary>
        /// <param name="idBaseStation"><the id(that was enterd by the user in main) of the basestation that the user wants to print/param>
        /// <returns>resturn the base station that needs to be printed</returns>
        public static BaseStation PrintBaseStation(int idBaseStation)
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
        public static Drone PrintDrone(int idDrone)
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
        public static Customer PrintCustomer(int idCustomer)
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
        public static Parcel PrintParcel(int idParcel)
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
        public static void DronToAParcel(int droneId, int parcelId)
            {
                int pareclIndex = 0;
                while (DataSource.Parcels[pareclIndex].Id != parcelId)//search for the parcel that has the same id has the id that the user enterd
                    pareclIndex++;
                DataSource.Parcels[pareclIndex].DroneId = droneId;
                DataSource.Parcels[pareclIndex].Scheduled = DateTime.Now;
            }

        /// <summary>
        /// update the drone that has the same id as the user enterd to charge in the base station that has the id that the user enterd
        /// </summary>
        /// <param name="dronesId">the drones id that the user enterd</param>
        /// <param name="idOfBaseStation">the base station id that the user enterd</param>
        public static void DronToCharger(int dronesId, int idOfBaseStation)
            {
                int droneIndex = 0;
                while (DataSource.Drones[droneIndex].Id != dronesId)//search for the drone that has the same id has the id that the user enterd
                    droneIndex++;
                int stationsIndex = 0;
                while (DataSource.Stations[stationsIndex].Id != idOfBaseStation)//search for the base station that has the same id has the id that the user enterd
                    stationsIndex++;
                DataSource.DroneCharges[DataSource.Config.DroneChargesIndex].DroneId = dronesId;
                DataSource.DroneCharges[DataSource.Config.DroneChargesIndex].StationId = DataSource.Stations[stationsIndex].Id;
                DataSource.Config.DroneChargesIndex++;
                DataSource.Stations[stationsIndex].EmptyCharges--;
                DataSource.Drones[droneIndex].Status = (DroneStatuses)1;//infix
            }

        /// <summary>
        /// free the drone-that has the same id that the user enterd from the charger that he was cherging from in the base station
        /// </summary>
        /// <param name="dronesId">the drones id that the user enterd</param>
        public static void FreeDroneFromBaseStation(int dronesId)
            {
                int droneLocation = 0;
                while (DataSource.Drones[droneLocation].Id != dronesId)//search for the drone that has the same id has the id that the user enterd
                    droneLocation++;
                DataSource.Drones[droneLocation].Battery = 100;
                DataSource.Drones[droneLocation].Status = (DroneStatuses)0;//availabel
                int droneChargesIndex = 0;
                while (DataSource.DroneCharges[droneChargesIndex].DroneId != dronesId)//search for the drone charge that is chargine the drone that has the same id as the user enterd
                    droneChargesIndex++;
                DataSource.DroneCharges[droneChargesIndex].DroneId = 0;//frees the charger
                int stationLocation = 0;
                while (DataSource.Stations[stationLocation].Id != DataSource.DroneCharges[droneChargesIndex].StationId)//search for the base station that has the same id as the one in the chrger
                    stationLocation++;
                DataSource.Stations[stationLocation].EmptyCharges++;
                DataSource.DroneCharges[droneChargesIndex].StationId = 0;
            }

        /// <summary>
        /// updates the time that the parcel was picked up by the dron
        /// </summary>
        /// <param name="newId">the id pf the parcel that was enterd by the user</param>
        public static void PickUpParcel(int newId)
            {
                int parcelsIndex = 0;
                while (DataSource.Parcels[parcelsIndex].Id != newId)//search for the parcel that has the same id has the id that the user enterd
                    parcelsIndex++;
                DataSource.Parcels[parcelsIndex].PickedUp = DateTime.Now;
            }

        /// <summary>
        /// updates the time that the parcel was deleverd to the customer
        /// </summary>
        /// <param name="newId">the id pf the parcel that was enterd by the user</param>
        public static void ParcelToCustomer(int newId)
            {
                int parcelsIndex = 0;
                while (DataSource.Parcels[parcelsIndex].Id != newId)//search for the parcel that has the same id has the id that the user enterd
                    parcelsIndex++;
                DataSource.Parcels[parcelsIndex].Delivered = DateTime.Now;
                DataSource.Parcels[parcelsIndex].DroneId = 0;
        }

        /// <summary>
        /// copyes the values of al the base stations in order to print them
        /// </summary>
        /// <returns>the new arrey that has the the base stations</returns>
        public static BaseStation[] PrintBaseStations()
            {
                BaseStation[] ActiveStations = new BaseStation[DataSource.Config.StationsIndex];
                for (int i = 0; i < DataSource.Config.StationsIndex; i++)//to copy all of the base station to the new arrey
                {
                    ActiveStations[i]= DataSource.Stations[i];
                }
                return ActiveStations;
            }

        /// <summary>
        /// copyes the values of all the drones in order to print them
        /// </summary>
        /// <returns>the new arrey that has the the drones</returns>
        public static Drone[] PrintDrones()
        {
            Drone[] ActiveDrones = new Drone[DataSource.Config.DronesIndex];
            for (int i = 0; i < DataSource.Config.DronesIndex; i++)//to copy all of the drones to the new arrey
            {
                ActiveDrones[i] = DataSource.Drones[i];
            }
            return ActiveDrones;
        }

        /// <summary>
        /// copyes the values of all the customer in order to print them
        /// </summary>
        /// <returns>the new arrey that has the the customers</returns>
        public static Customer[] PrintCustomers()
        {
            Customer[] ActiveCustomers = new Customer[DataSource.Config.CustomersIndex];
            for (int i = 0; i < DataSource.Config.CustomersIndex; i++)//to copy all of the customers to the new arrey
            {
                ActiveCustomers[i]= DataSource.Customers[i];
            }
            return ActiveCustomers;
        }

        /// <summary>
        /// copyes the values of all the parcel in order to print them
        /// </summary>
        /// <returns>the new arrey that has the the parceles</returns>
        public static Parcel[] PrintPercels()
        {
            Parcel[] ActiveParcels = new Parcel[DataSource.Config.ParcelsIndex];
            for (int i = 0; i < DataSource.Config.ParcelsIndex; i++)//to copy all of the parceles to the new arrey
            {
                ActiveParcels[i]= DataSource.Parcels[i];
            }
            return ActiveParcels;
        }

        /// <summary>
        /// search for all the drones that are available and copy them to a new arrey  
        /// </summary>
        /// <returns>the new arrey that has all the drones that are availeble</returns>
        public static Parcel[] ParcelThatWerenNotPaired()
            {
                int count = 0;
                for (int i = 0; i < DataSource.Config.ParcelsIndex; i++)//counts how many available drones ther are
                    if (DataSource.Parcels[i].DroneId == 0)
                        count++;
                Parcel[] Parcels = new Parcel[count];//makes a new arrey in the size of the available drones
                for (int i = 0; i < DataSource.Config.ParcelsIndex; i++)//to copy all the available drones
                    if (DataSource.Parcels[i].DroneId == 0)//if the dron is available copy his dalaies
                    {
                        Parcels[i]= DataSource.Parcels[i];
                    }
                return Parcels;
            }

        /// <summary>
        /// search for tha base station that has available charges and copys them to a new arrey
        /// </summary>
        /// <returns>the new arrey that has all the base station that has free charges</returns>
        public static BaseStation[] BaseStationWithAvailableCharges()
            {
                int count = 0;
                for (int i = 0; i < DataSource.Config.StationsIndex; i++)//checks all the base station for available charges
                    if (DataSource.Stations[i].EmptyCharges > 0)//meens ther are enmpty charges
                        count++;
                BaseStation[] Stations = new BaseStation[count];//a new arrey in the size of the base station that have available charges
                for (int i = 0; i < DataSource.Config.StationsIndex; i++)//checks all the base station for available charges
                    if (DataSource.Stations[i].EmptyCharges > 0)//meens ther are enmpty charges so copy the base station
                    {
                       Stations[i] = DataSource.Stations[i];
                    }
                return Stations;
            }
    }
}

     



