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
        public double[] AskForBattery();
        public void AddBaseStation(BaseStation addBaseStation);
        public void AddDrone(Drone addDrone);
        public void AddCustomer(Customer newCustomer);
        public void AddParcel(Parcel newParcel);
        public void AddDroneCharge(DroneCharge droneCharge);
        public BaseStation GetBaseStation(int idBaseStation);
        public Drone GetDrone(int idDrone);
        public Customer GetCustomer(int idCustomer);
        public Parcel GetParcel(int idParcel);
        public void DronToAParcel(int droneId, int parcelId);
        public void DronToCharger(int dronesId, int idOfBaseStation);
        public void FreeDroneFromBaseStation(int dronesId);
        public void PickUpParcel(int newId);
        public void ParcelToCustomer(int newId);
       IEnumerable<BaseStation> GetBaseStations();
       IEnumerable<Drone> GetDrones();
       IEnumerable<Customer> GetCustomers();
       IEnumerable<Parcel> GetParcels();
       IEnumerable<DroneCharge> GetDroneCharge();
       IEnumerable<Parcel> GetParcelThatWerenNotPaired();
       IEnumerable<BaseStation> GetBaseStationWithAvailableCharges();
        public void UpdateCustomer(int id, string name, string phone);
    }
}
