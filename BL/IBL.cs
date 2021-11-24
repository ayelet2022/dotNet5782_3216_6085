using IBL.BO;
using System;
using System.Collections.Generic;

namespace IBL
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
        void AddParcel(Parcel parcel);

        /// <summary>
        /// a drone deliverd a parcel
        /// </summary>
        /// <param name="id">the is of the drone that deliverd the parcel</param>
        void DeliverParcel(int id);
        double DisDronToBS(Customer customer, BaseStation station);
        double DisDronToCustomer(DroneList drone, Customer customer);

        /// <summary>
        /// returne the distance between the sender of the parcel to the resever of the parcel
        /// </summary>
        /// <param name="sender">the customer that send the parcel</param>
        /// <param name="resever">the customer that suppsed to resev the parcel</param>
        /// <returns></returns>
        double DisSenderToResever(Customer sender, Customer resever);

        /// <summary>
        /// fineds the closest base station to the customer
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns>the closest base station</returns>
        IDAL.DO.BaseStation FindMinDistanceOfCToBS(double latitude, double longitude);
        IDAL.DO.BaseStation FindMinDistanceOfDToBS(Drone drone);

        /// <summary>
        /// to free a drone from a charger
        /// </summary>
        /// <param name="id">the id of the  drone we want to free</param>
        /// <param name="timeInCharger">how long was the drone charging</param>
        void FreeDroneFromeCharger(int id, double timeInCharger);

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
        IEnumerable<BaseStationList> GetBaseStations();

        /// <summary>
        /// search for tha base station that has available charges and copys them to a new list
        /// </summary>
        /// <returns>the new list that has all the base station that has free charges</returns>
        IEnumerable<BaseStationList> GetBaseStationWithAvailableCharges();

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
        IEnumerable<CustomerList> GetCustomers();

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
        IEnumerable<DroneList> GetDrones();
        Parcel GetParcel(int idParcel);
        IEnumerable<ParcelList> GetParcels();
        IEnumerable<ParcelList> GetParcelThatWerenNotPaired();

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
        void UpdateDrone(int id, string newModel);

        /// <summary>
        /// update the base station that has the id that was enterd
        /// </summary>
        /// <param name="id">the id of the base station that we want to update</param>
        /// <param name="newName">the new name to the base station </param>
        /// <param name="emptyCharges">the new emount of empty chargers in the base station</param>
        void UpdateStation(int id, string newName, int emptyCharges);
        public int UseOfBattery(IDAL.DO.Parcel parcel, DroneList drone);

    }
}