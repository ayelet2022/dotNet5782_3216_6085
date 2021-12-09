using BO;
using System;
using System.Collections.Generic;

namespace BlApi
{
    public interface IBL
    {
        /// <summary>
        /// adds a new base station to the list
        /// </summary>
        /// <param name="baseStation">the new base station that we want to add</param>
        void AddBaseStation(BaseStation baseStation);

        /// <summary>
        /// adds a new customer to the list of customers
        /// </summary>
        /// <param name="customer">the new customer we want to add</param>
        void AddCustomer(Customer customer);

        /// <summary>
        /// adds a new drone to the list
        /// </summary>
        /// <param name="drone">the drone to add</param>
        /// <param name="idFirstStation">the id of the first station that the drone is in</param>
        void AddDrone(Drone drone, int idFirstStation);
        /// <summary>
        /// adds a new parcel to the list of parcels
        /// </summary>
        /// <param name="parcel">the new parcel we want to add</param>
        void AddParcel(Parcel parcel);

        /// <summary>
        /// a drone deliverd a parcel
        /// </summary>
        /// <param name="id">the is of the drone that deliverd the parcel</param>
        void DeliverParcel(int id);

        /// <summary>
        /// to free the drone from the charger
        /// </summary>
        /// <param name="id">the drone we want to free from the charger</param>
        /// <param name="timeInCharger">how long was the drone charging for</param>
        void FreeDroneFromeCharger(int id);

        /// <summary>
        /// returens the besa station that has the id that was enterd 
        /// </summary>
        /// <param name="idBaseStation">the id of the base station that we want to retirne</param>
        /// <returns>the base station that has the same id</returns>
        BaseStation GetBaseStation(int idBaseStation);

        /// <summary>
        /// returens the all base station list
        /// </summary>
        /// <returns>the all base station list</returns>
        IEnumerable<BaseStationList> GetBaseStations(Predicate<BaseStationList> predicate=null);

        /// <summary>
        /// returne the customer with the id that was enterd
        /// </summary>
        /// <param name="idCustomer">the id of the customer we want to returne</param>
        /// <returns>the customer with the id</returns>
        Customer GetCustomer(int idCustomer);

        /// <summary>
        /// returne the all list of the customers
        /// </summary>
        /// <returns>the new list of the customers</returns>
        IEnumerable<CustomerList> GetCustomers(Predicate<CustomerList> predicate = null);

        /// <summary>
        /// returne the drone that has the same id as what wes enterd
        /// </summary>
        /// <param name="idDrone">the id of the drone we want to returne</param>
        /// <returns>the drone that has the same id</returns>
        Drone GetDrone(int idDrone);

        /// <summary>
        /// copyes the values of all the drones in order to print them
        /// </summary>
        /// <returns>the new arrey that has the the drones</returns>
        IEnumerable<DroneList> GetDrones(Predicate<DroneList> predicate = null);
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
        IEnumerable<ParcelList> GetParcels(Predicate<ParcelList> predicate = null);

        /// <summary>
        /// a drone piched up a parcel
        /// </summary>
        /// <param name="id">the id of the drone that picked up the parcel</param>
        void PickUpParcel(int id);

        /// <summary>
        /// to paire a parcel to the drone that has the same id as what was enterd
        /// </summary>
        /// <param name="droneId">the drone we want to paire a parcel to</param>
        void ScheduledAParcelToADrone(int droneId);

        /// <summary>
        /// sends the drone with the id that was enterd to a charger
        /// </summary>
        /// <param name="id">the id of the drone that we want to send to charging</param>
        void SendDroneToCharging(int id);

        /// <summary>
        /// updates the customer with the id that was enterd
        /// </summary>
        /// <param name="id">the id of the customet we want to update</param>
        /// <param name="name">the new name to the customer</param>
        /// <param name="phone">the new phone to the customer</param>
        void UpdateCustomer(int id, string name, string phone);

        /// <summary>
        /// updates the drone that has the same id that was enterd
        /// </summary>
        /// <param name="id">the id of the drone we want to update</param>
        /// <param name="newModel">the new model of the drone</param>
        void UpdateDrone(Drone drone, string newModel);

        /// <summary>
        /// update the base station that has the id that was enterd
        /// </summary>
        /// <param name="id">the id of the base station that we want to update</param>
        /// <param name="newName">the new name to the base station </param>
        /// <param name="emptyCharges">the new emount of empty chargers in the base station</param>
        void UpdateStation(int id, string newName, int emptyCharges);

    }
}