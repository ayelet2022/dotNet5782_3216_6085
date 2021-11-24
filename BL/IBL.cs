using IBL.BO;
using System;
using System.Collections.Generic;

namespace IBL
{
    public interface IBL
    {
        void AddBaseStation(BaseStation baseStation);
        void AddCustomer(Customer customer);
        void AddDrone(Drone drone, int idFirstStation);
        /// <summary>
        /// adds a new parcel to the list of parcels
        /// </summary>
        /// <param name="parcel">the new parcel we want to add</param>
        void AddParcel(Parcel parcel);
        void DeliverParcel(int id);
        void FreeDroneFromeCharger(int id, double timeInCharger);
        BaseStation GetBaseStation(int idBaseStation);
        IEnumerable<BaseStationList> GetBaseStations();
        IEnumerable<BaseStationList> GetBaseStationWithAvailableCharges();
        Customer GetCustomer(int idCustomer);
        IEnumerable<CustomerList> GetCustomers();
        Drone GetDrone(int idDrone);
        IEnumerable<DroneList> GetDrones();
        /// <summary>
        /// returnes the parcel that has the same id has wat was enterd
        /// </summary>
        /// <param name="idParcel">the id of the parcel we want to reurne</param>
        /// <returns>the parcel that has the same id that enterd</returns>
        Parcel GetParcel(int idParcel);
        /// <summary>
        /// copyes the values of all the parcel in order to print them
        /// </summary>
        /// <returns>the new arrey that has the the parceles</returns>
        IEnumerable<ParcelList> GetParcels();
        /// <summary>
        /// search for all the drones that are available and copy them to a new arrey  
        /// </summary>
        /// <returns>the new arrey that has all the drones that are availeble</returns>
        IEnumerable<ParcelList> GetParcelThatWerenNotPaired();
        void PickUpParcel(int id);
        void ScheduledAParcelToADrone(int droneId);
        void SendDroneToCharging(int id);
        void UpdateCustomer(int id, string name, string phone);
        void UpdateDrone(int id, string newModel);
        void UpdateStation(int id, string newName, int emptyCharges);

    }
}