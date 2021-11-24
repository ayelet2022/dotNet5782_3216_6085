using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
using IDAL.DO;
namespace IDAL
{
    public interface IDal
    {
        /// <summary>
        /// returnes an arrey of the battery use in diffrent kindes of delivery/flights 
        /// </summary>
        /// <returns>the arrey that has the data of the battery use of the drones</returns>
        public double[] AskForBattery();

        /// <summary>
        /// adds a new base station to the arrey
        /// </summary>
        public void AddBaseStation(BaseStation addBaseStation);

        /// <summary>
        /// adds a new drone to the arrey
        /// </summary>
        public void AddDrone(Drone addDrone);

        /// <summary>
        /// adds a new customer to the arrey
        /// </summary>
        /// <param name="newCustomer">the new customer that the user entered in main and needs to be added to the arrey</param>
        public void AddCustomer(Customer newCustomer);
        /// <summary>
        /// adds a new parcel to the arrey
        /// </summary>
        /// <param name="newParcel">the new parce that the user entered in main and needs to be added to the arrey</param>
        public void AddParcel(Parcel newParcel);

        /// <summary>
        /// adds a new charger to the list of chargers
        /// </summary>
        /// <param name="droneCharge">the new charager we want to add</param>
        public void AddDroneCharge(DroneCharge droneCharge);
        /// <summary>
        ///  search for the base station in the arrey thet has the same id that the user enterd and returnes it
        /// </summary>
        /// <param name="idBaseStation"><the id(that was enterd by the user in main) of the basestation that the user wants to print/param>
        /// <returns>resturn the base station that needs to be printed</returns>
        public BaseStation GetBaseStation(int idBaseStation);

        /// <summary>
        ///  search for the drone in the arrey thet has the same id as the user enterd and returnes it
        /// </summary>
        /// <param name="idDrone">the id(that was enterd by the user in main) of the dron that the user wants to print</param>
        /// <returns>resturn the drone that needs to be printed</returns>
        public Drone GetDrone(int idDrone);

        /// <summary>
        /// search for the customer in the arrey thet has the same id as the user enterd and retirnes it
        /// </summary>
        /// <param name="idCustomer">the id(that was enterd by the user in main) of the customer that the user wants to print</param>
        /// <returns>resturn the customer that needs to be printed</returns>
        public Customer GetCustomer(int idCustomer);

        /// <summary>
        /// search for the parcel in the arrey thet has the same id as the user enterd and retirnes it
        /// </summary>
        /// <param name="idParcel">the id(that was enterd by the user in main) of the parcel that the user wants to print</param> 
        /// <returns>the parcel that needs to be printed</returns>
        public Parcel GetParcel(int idParcel);

        /// <summary>
        /// search for the drone and the parcel that has the same id as what the user enterd 
        /// and changes the filed of the drone id inn the parcel detailes
        /// </summary>
        /// <param name="droneId">the drones id that the user enterd</param>
        /// <param name="parcelId">the parcel id that the user enterd</param>
        public void DronToAParcel(int droneId, int parcelId);

        /// <summary>
        /// update the drone that has the same id as the user enterd to charge in the base station that has the id that the user enterd
        /// </summary>
        /// <param name="dronesId">the drones id that the user enterd</param>
        /// <param name="idOfBaseStation">the base station id that the user enterd</param>
        public void DronToCharger(int dronesId, int idOfBaseStation);

        /// <summary>
        /// free the drone-that has the same id that the user enterd from the charger that he was cherging from in the base station
        /// </summary>
        /// <param name="dronesId">the drones id that the user enterd</param>
        public void FreeDroneFromBaseStation(int dronesId);

        /// <summary>
        /// updates the time that the parcel was picked up by the dron
        /// </summary>
        /// <param name="newId">the id pf the parcel that was enterd by the user</param>
        public void PickUpParcel(int newId);

        /// <summary>
        /// updates the time that the parcel was deleverd to the customer
        /// </summary>
        /// <param name="newId">the id pf the parcel that was enterd by the user</param>
        public void ParcelToCustomer(int newId);

        /// <summary>
        /// copyes the values of al the base stations in order to print them
        /// </summary>
        /// <returns>the new arrey that has the the base stations</returns>
        IEnumerable<BaseStation> GetBaseStations();

        /// <summary>
        /// copyes the values of all the drones in order to print them
        /// </summary>
        /// <returns>the new arrey that has the the drones</returns>
        IEnumerable<Drone> GetDrones();

        /// <summary>
        /// returnes the list of customers
        /// </summary>
        /// <returns>the list of costomers</returns>
        IEnumerable<Customer> GetCustomers();

        /// <summary>
        /// copyes the values of all the parcel in order to print them
        /// </summary>
        /// <returns>the new arrey that has the the parceles</returns>
        IEnumerable<Parcel> GetParcels();

        /// <summary>
        /// returne the list of all the chargers
        /// </summary>
        /// <returns>the list of the chargers</returns>
        IEnumerable<DroneCharge> GetDroneCharge();

        /// <summary>
        /// search for all the drones that are available and copy them to a new arrey  
        /// </summary>
        /// <returns>the new arrey that has all the drones that are availeble</returns>
        IEnumerable<Parcel> GetParcelThatWerenNotPaired();

        /// <summary>
        /// search for tha base station that has available charges and copys them to a new arrey
        /// </summary>
        /// <returns>the new arrey that has all the base station that has free charges</returns>
        IEnumerable<BaseStation> GetBaseStationWithAvailableCharges();

        /// <summary>
        /// to update the customer that has the same id-change his name/his phone
        /// </summary>
        /// <param name="id">the id of the customer we want to update</param>
        /// <param name="name">the new name we want to give the customer</param>
        /// <param name="phone">the new phone we want to give the customer</param>
        public void UpdateCustomer(int id, string name, string phone);

        /// <summary>
        /// to update the drone that has the same id-change his model
        /// </summary>
        /// <param name="id">the drone we want to update</param>
        /// <param name="newModel">the new model we want to give the drone</param>
        public void UpdateDrone(int id, string newModel);

        /// <summary>
        /// to update the base station that has the same id-change the name/the num of empty chargers
        /// </summary>
        /// <param name="id">the id of the base station we want to change</param>
        /// <param name="newName">the new name we want to give the station</param>
        /// <param name="emptyCharges">the new emount of chargers in base station</param>
        public void UpdateStation(int id, string newName, int emptyCharges);

        /// <summary>
        /// returnes the list of the id of the customers that reseved a parcel
        /// </summary>
        /// <returns>all the id of customers that reseved parcels</returns>
        public IEnumerable<int> GetCustomersRe();

        /// <summary>
        /// update the charging positions of the base station 
        /// </summary>
        /// <param name="baseStation">the base station we want to update</param>

    }
}
