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
        void AddParcel(Parcel parcel);
        void DeliverParcel(int id);
        double DisDronToBS(Customer customer, BaseStation station);
        double DisDronToCustomer(DroneList drone, Customer customer);
        double DisSenderToResever(Customer sender, Customer resever);
        IDAL.DO.BaseStation FindMinDistanceOfCToBS(double latitude, double longitude);
        IDAL.DO.BaseStation FindMinDistanceOfDToBS(Drone drone);
        void FreeDroneFromeCharger(int id, double timeInCharger);
        BaseStation GetBaseStation(int idBaseStation);
        IEnumerable<BaseStationList> GetBaseStations();
        IEnumerable<BaseStationList> GetBaseStationWithAvailableCharges();
        Customer GetCustomer(int idCustomer);
        IEnumerable<CustomerList> GetCustomers();
        Drone GetDrone(int idDrone);
        IEnumerable<DroneList> GetDrones();
        Parcel GetParcel(int idParcel);
        IEnumerable<ParcelList> GetParcels();
        IEnumerable<ParcelList> GetParcelThatWerenNotPaired();
        void PickUpParcel(int id);
        void ScheduledAParcelToADrone(int droneId);
        void SendDroneToCharging(int id);
        void UpdateCustomer(int id, string name, string phone);
        void UpdateDrone(int id, string newModel);
        void UpdateStation(int id, string newName, int emptyCharges);
        public int UseOfBattery(IDAL.DO.Parcel parcel, DroneList drone);

    }
}