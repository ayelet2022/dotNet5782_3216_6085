using System;
using DAL.IDAL.DO;
namespace DAL
{
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
            public static void AddBaseStation()
            {
                Random Rand = new Random(DateTime.Now.Millisecond);
                DataSource.Stations[DataSource.Config.StationsIndex].Id = Rand.Next(1000, 10000);
                DataSource.Stations[DataSource.Config.StationsIndex].Latitude = Rand.Next(0, 1000000);
                DataSource.Stations[DataSource.Config.StationsIndex].Latitude = Rand.Next(0, 1000000);
                DataSource.Stations[DataSource.Config.StationsIndex].EmptyCharges = Rand.Next(0, 6);
                DataSource.Stations[DataSource.Config.StationsIndex].Name = "ccc";
                DataSource.Config.StationsIndex++;
            }
            /// <summary>
            /// adds a new drone to the arrey
            /// </summary>
            public static void AddDrone()
            {
                Random Rand = new Random(DateTime.Now.Millisecond);
                DataSource.Drones[DataSource.Config.DronesIndex].Id = Rand.Next(1000, 10000);
                DataSource.Drones[DataSource.Config.DronesIndex].MaxWeight = (WeightCategories)Rand.Next(0, 3);
                DataSource.Drones[DataSource.Config.DronesIndex].Status = (DroneStatuses)Rand.Next(0, 3);
                DataSource.Drones[DataSource.Config.DronesIndex].Battery = Rand.Next(0, 101);
                DataSource.Drones[DataSource.Config.DronesIndex].Model = "x";
                DataSource.Config.DronesIndex++;
            }
            /// <summary>
            /// adds a new customer to the arrey
            /// </summary>
            /// <param name="newCustomer"></param>
            public static void AddCustomer(Customer newCustomer)
            {
                DataSource.Customers[DataSource.Config.CustomersIndex].Id = newCustomer.Id;
                DataSource.Customers[DataSource.Config.CustomersIndex].Latitude = newCustomer.Latitude;
                DataSource.Customers[DataSource.Config.CustomersIndex].Longitude = newCustomer.Longitude;
                DataSource.Customers[DataSource.Config.CustomersIndex].Name = newCustomer.Name;
                DataSource.Customers[DataSource.Config.CustomersIndex].Phone = newCustomer.Phone;
                DataSource.Config.CustomersIndex++;
            }
            /// <summary>
            /// adds a new parcel to the arrey
            /// </summary>
            /// <param name="newParcel"></param>
            public static void AddParcel(Parcel newParcel)
            {
                DataSource.Parcels[DataSource.Config.ParcelsIndex].CreatParcel = DateTime.Now;
                DataSource.Parcels[DataSource.Config.ParcelsIndex].Id = DataSource.Config.RunningParcelId++;
                DataSource.Parcels[DataSource.Config.ParcelsIndex].SenderId = newParcel.SenderId;
                DataSource.Parcels[DataSource.Config.ParcelsIndex].TargetId = newParcel.TargetId;
                DataSource.Parcels[DataSource.Config.ParcelsIndex].DroneId = 0;
                DataSource.Parcels[DataSource.Config.ParcelsIndex].Scheduled = new(0);
                DataSource.Parcels[DataSource.Config.ParcelsIndex].PickedUp =new(0);
                DataSource.Parcels[DataSource.Config.ParcelsIndex].Delivered = new(0);
                DataSource.Parcels[DataSource.Config.ParcelsIndex].Priority = newParcel.Priority;
                DataSource.Parcels[DataSource.Config.ParcelsIndex].Weight = newParcel.Weight;
                DataSource.Config.ParcelsIndex++;
            }
            /// <summary>
            ///  returnes the base sation that the user wants to print according to his id
            /// </summary>
            /// <param name="idBaseStation"></param>
            /// <returns></returns>
            public static BaseStation PrintBaseStation(int idBaseStation)
            {
                int baseStationIndex = 0;
                while (DataSource.Stations[baseStationIndex].Id != idBaseStation)
                    baseStationIndex++;
                return (DataSource.Stations[baseStationIndex]);

            }

            /// <summary>
            ///  returnes the drone that the user wantes to print according to his id
            /// </summary>
            /// <param name="idDrone"></param>
            /// <returns></returns>
            public static Drone PrintDrone(int idDrone)
            {
                int droneIndex = 0;
                while (DataSource.Drones[droneIndex].Id != idDrone)
                    droneIndex++;
                return (DataSource.Drones[droneIndex]);

            }

            public static Customer PrintCustomer(int idCustomer)
            {
                int customerIndex = 0;
                while (DataSource.Customers[customerIndex].Id != idCustomer)
                    customerIndex++;
                return (DataSource.Customers[customerIndex]);

            }
            /// <summary>
            /// returnes the parcel that the user wanted to print according to his id
            /// </summary>
            /// <param name="idParcel"></param> the id of the parcel the the user wants to print
            /// <returns></returns>the parcel that needs to be printed
            public static Parcel PrintParcel(int idParcel)
            {
                int parcelIndex = 0;
                while (DataSource.Parcels[parcelIndex].Id != idParcel)
                    parcelIndex++;
               return (DataSource.Parcels[parcelIndex]);

            }
            /// <summary>
            /// looks for a free drone that can the delever the parcel to the customer
            /// </summary>
            /// <param name="newParcel"></param> the parcel thats the dron need to deliver
            /// <returns></returns>the id of the drone that can deliver the parcel
            public static void DronToAParcel(int droneId, int parcelId)
            {
                int pareclIndex = 0;
                while (DataSource.Parcels[pareclIndex].Id != parcelId)
                    pareclIndex++;
                int droneInsex = 0;
                while (DataSource.Drones[droneInsex].Id != droneId)
                    droneInsex++;
                DataSource.Parcels[pareclIndex].DroneId = DataSource.Drones[droneInsex].Id;
                DataSource.Parcels[pareclIndex].Scheduled = DateTime.Now;
            }

            /// <summary>
            /// looks for an empty charger in the base station so the drone will be able to charge there 
            /// and updates all that needs to be update becaus of that
            /// </summary>
            /// <param name="dronToCharge"></param> the drone that need charging
            public static void DronToCharger(int dronesId, int idOfBaseStation)
            {
                int droneIndex = 0;
                while (DataSource.Drones[droneIndex].Id != dronesId)
                    droneIndex++;
                int stationsIndex = 0;
                while (DataSource.Stations[stationsIndex].Id != idOfBaseStation)
                    stationsIndex++;
                DataSource.DroneCharges[DataSource.Config.DroneChargesIndex].DroneId = DataSource.Drones[droneIndex].Id;
                DataSource.DroneCharges[DataSource.Config.DroneChargesIndex].StationId = DataSource.Stations[stationsIndex].Id;
                DataSource.Config.DroneChargesIndex++;
                DataSource.Stations[stationsIndex].EmptyCharges--;
                DataSource.Drones[droneIndex].Status = (DroneStatuses)1;
            }

            /// <summary>
            /// free the drone from the charger that he was cherging from in the base station
            /// </summary>
            /// <param name="dronToFree"></param>the drone that needs to get free from the charger he his charging from
            public static void FreeDroneFromBaseStation(int dronesId)
            {
                int droneLocation = 0;
                while (DataSource.Drones[droneLocation].Id != dronesId)
                    droneLocation++;
                DataSource.Drones[droneLocation].Battery = 100;
                DataSource.Drones[droneLocation].Status = (DroneStatuses)0;
                int droneChargesIndex = 0;
                while (DataSource.DroneCharges[droneChargesIndex].DroneId != dronesId)
                    droneChargesIndex++;
                DataSource.DroneCharges[droneChargesIndex].DroneId = 0;
                int stationLocation = 0;
                while (DataSource.Stations[stationLocation].Id != DataSource.DroneCharges[droneChargesIndex].StationId)
                    stationLocation++;
                DataSource.Stations[stationLocation].EmptyCharges++;
                DataSource.DroneCharges[droneChargesIndex].StationId = 0;
            }

            /// <summary>
            /// updates the time that the parcel was picked up by the dron
            /// </summary>
            /// <param name="ParclToPickup"></param> the parcel that needs delevering
            public static void PickUpParcel(int newId)
            {
                int parcelsIndex = 0;
                while (DataSource.Parcels[parcelsIndex].Id != newId)
                    parcelsIndex++;
                DataSource.Parcels[parcelsIndex].PickedUp = DateTime.Now;
            }

            /// <summary>
            /// updates the time that the parcel was deleverd to the customer
            /// </summary>
            /// <param name="ParcelDeliverd"></param>
            public static void ParcelToCustomer(int newId)
            {
                int parcelsIndex = 0;
                while (DataSource.Parcels[parcelsIndex].Id != newId)
                    parcelsIndex++;
                DataSource.Parcels[parcelsIndex].Delivered = DateTime.Now;
            }


            /// <summary>
            /// prints the whole list of the object according to what the user enterd
            /// </summary>
            public static BaseStation[] PrintBaseStations()
            {
                BaseStation[] ActiveStations = new BaseStation[DataSource.Config.StationsIndex]; 
                for (int i = 0; i < DataSource.Config.StationsIndex; i++)
                {
                    ActiveStations[i].Id = DataSource.Stations[i].Id;
                    ActiveStations[i].Name = DataSource.Stations[i].Name;
                    ActiveStations[i].EmptyCharges = DataSource.Stations[i].EmptyCharges;
                    ActiveStations[i].Latitude = DataSource.Stations[i].Latitude;
                    ActiveStations[i].Longitude = DataSource.Stations[i].Longitude;
                }
                return ActiveStations;
            }
            public static Drone[] PrintDrones()
            {
                Drone[] ActiveDrones = new Drone[DataSource.Config.DronesIndex];
                for (int i = 0; i < DataSource.Config.DronesIndex; i++)
                {
                    ActiveDrones[i].Id = DataSource.Drones[i].Id;
                    ActiveDrones[i].MaxWeight = DataSource.Drones[i].MaxWeight;
                    ActiveDrones[i].Model = DataSource.Drones[i].Model;
                    ActiveDrones[i].Status = DataSource.Drones[i].Status;
                    ActiveDrones[i].Battery = DataSource.Drones[i].Battery;
                }
                return ActiveDrones;
            }
            public static Customer[] PrintCustomers()
            {
                Customer[] ActiveCustomers = new Customer[DataSource.Config.CustomersIndex];
                for (int i = 0; i < DataSource.Config.CustomersIndex; i++)
                {
                    ActiveCustomers[i].Id = DataSource.Customers[i].Id;
                    ActiveCustomers[i].Latitude = DataSource.Customers[i].Latitude;
                    ActiveCustomers[i].Longitude = DataSource.Customers[i].Longitude;
                    ActiveCustomers[i].Name = DataSource.Customers[i].Name;
                    ActiveCustomers[i].Phone = DataSource.Customers[i].Phone;
                }
                return ActiveCustomers;
            }
            public static Parcel[] PrintPercels()
            {
                Parcel[] ActiveParcels = new Parcel[DataSource.Config.ParcelsIndex];
                for (int i = 0; i < DataSource.Config.ParcelsIndex; i++)
                {
                    ActiveParcels[i].Id = DataSource.Parcels[i].Id;
                    ActiveParcels[i].PickedUp = DataSource.Parcels[i].PickedUp;
                    ActiveParcels[i].Priority = DataSource.Parcels[i].Priority;
                    ActiveParcels[i].Scheduled = DataSource.Parcels[i].Scheduled;
                    ActiveParcels[i].SenderId = DataSource.Parcels[i].SenderId;
                    ActiveParcels[i].TargetId = DataSource.Parcels[i].TargetId;
                    ActiveParcels[i].Weight = DataSource.Parcels[i].Weight;
                    ActiveParcels[i].DroneId = DataSource.Parcels[i].DroneId;
                    ActiveParcels[i].Delivered = DataSource.Parcels[i].Delivered;
                    ActiveParcels[i].CreatParcel = DataSource.Parcels[i].CreatParcel;
                }
                return ActiveParcels;
            }
            public static Parcel[] ParcelThatWerenNotPaired()
            {
                int count = 0;
                for (int i = 0; i < DataSource.Config.ParcelsIndex; i++)
                    if (DataSource.Parcels[i].DroneId == 0)
                        count++;
                Parcel[] Parcels = new Parcel[count];
                for (int i = 0; i < DataSource.Config.ParcelsIndex; i++)
                    if (DataSource.Parcels[i].DroneId == 0)
                    {
                        Parcels[i].Id = DataSource.Parcels[i].Id;
                        Parcels[i].PickedUp = DataSource.Parcels[i].PickedUp;
                        Parcels[i].Priority = DataSource.Parcels[i].Priority;
                        Parcels[i].Scheduled = DataSource.Parcels[i].Scheduled;
                        Parcels[i].SenderId = DataSource.Parcels[i].SenderId;
                        Parcels[i].TargetId = DataSource.Parcels[i].TargetId;
                        Parcels[i].Weight = DataSource.Parcels[i].Weight;
                        Parcels[i].DroneId = DataSource.Parcels[i].DroneId;
                        Parcels[i].Delivered = DataSource.Parcels[i].Delivered;
                        Parcels[i].CreatParcel = DataSource.Parcels[i].CreatParcel;
                    }
                return Parcels;
            }
            public static BaseStation[] BaseStationWithAvailableCharges()
            {
                int count = 0;
                for (int i = 0; i < DataSource.Config.StationsIndex; i++)
                    if (DataSource.Stations[i].EmptyCharges > 0)
                        count++;
                BaseStation[] Stations = new BaseStation[count];
                for (int i = 0; i < DataSource.Config.StationsIndex; i++)
                    if (DataSource.Stations[i].EmptyCharges > 0)
                    {
                       Stations[i].Id = DataSource.Stations[i].Id;
                       Stations[i].Name = DataSource.Stations[i].Name;
                       Stations[i].EmptyCharges = DataSource.Stations[i].EmptyCharges;
                       Stations[i].Latitude = DataSource.Stations[i].Latitude;
                       Stations[i].Longitude = DataSource.Stations[i].Longitude;
                    }
                return Stations;
            }
        }
        /// <summary>
        /// includs all the data of the object-the arr of each object,
        /// the index of each arr 
        /// </summary>
    class DataSource
        {
            internal static Drone[] Drones = new Drone[10];
            internal static BaseStation[] Stations = new BaseStation[5];
            internal static Customer[] Customers = new Customer[100];
            internal static Parcel[] Parcels = new Parcel[1000];
            internal static DroneCharge[] DroneCharges = new DroneCharge[25];
            internal class Config
            {
                internal static int DronesIndex = 0;
                internal static int StationsIndex = 0;
                internal static int CustomersIndex = 0;
                internal static int ParcelsIndex = 0;
                internal static int DroneChargesIndex = 0;
                internal static int RunningParcelId = 1000;
            }
            /// <summary>
            /// Incluods the data that we enterd
            /// </summary>
            internal static void Initialize()
            {
                Random Rand = new Random(DateTime.Now.Millisecond);
                for (int i = 0; i < 2; i++)
                {
                    Stations[i].Id = Rand.Next(1000, 10000);
                    Stations[i].Latitude = Rand.Next(0, 1000000);
                    Stations[i].Latitude = Rand.Next(0, 1000000);
                    Stations[i].EmptyCharges = Rand.Next(0,6);
                    DataSource.Config.StationsIndex++;
                }
                Stations[0].Name = "aaa";
                Stations[1].Name = "bbb";

                for (int i = 0; i < 5; i++)
                {
                    Drones[i].Id = Rand.Next(1000, 10000);
                    Drones[i].MaxWeight = (WeightCategories)Rand.Next(0,3);
                    Drones[i].Status = (DroneStatuses)Rand.Next(0,3);
                    DataSource.Config.DronesIndex++;
                }
                Drones[0].Model = "a";
                Drones[2].Model = "b";
                Drones[3].Model = "c";
                Drones[4].Model = "d";
                Drones[1].Model = "e";

                for (int i = 0; i < 10; i++)
                {
                    Customers[i].Id = Rand.Next(1000, 10000);
                    Customers[i].Latitude = Rand.Next(0, 1000000);
                    Customers[i].Longitude = Rand.Next(0, 1000000);
                    DataSource.Config.CustomersIndex++;
                }
                Customers[0].Name = "Ayelet";
                Customers[1].Name = "Penina";
                Customers[2].Name = "Yosi";
                Customers[3].Name = "Avi";
                Customers[4].Name = "Nomi";
                Customers[5].Name = "Michal";
                Customers[6].Name = "Daniel";
                Customers[7].Name = "Chaya";
                Customers[8].Name = "Chani";
                Customers[9].Name = "Yakov";

                Customers[0].Phone = "0511111111";
                Customers[1].Phone = "0522222222";
                Customers[2].Phone = "0533333333";
                Customers[3].Phone = "0544444444";
                Customers[4].Phone = "0555555555";
                Customers[5].Phone = "0566666666";
                Customers[6].Phone = "0577777777";
                Customers[7].Phone = "0588888888";
                Customers[8].Phone = "0599999999";
                Customers[9].Phone = "0500000000";

                for (int i = 0; i < 10; i++)
                {
                    Parcels[i].CreatParcel = new(0);
                    Parcels[i].Id = DataSource.Config.RunningParcelId++;
                    Parcels[i].Priority = (Priorities)Rand.Next(0,3);
                    Parcels[i].SenderId= Rand.Next(1000, 10000);
                    Parcels[i].TargetId = Rand.Next(1000, 10000);
                    Parcels[i].Weight = (WeightCategories)Rand.Next(0, 3);
                    Parcels[i].DroneId = 0;
                    Parcels[i].Delivered = new(0);
                    Parcels[i].PickedUp = new(0);
                    Parcels[i].Scheduled = new(0);
                    DataSource.Config.ParcelsIndex++;
                }
                Parcels[0].CreatParcel = new(2021, 3, 2, 8, 30, 0);
                Parcels[1].CreatParcel = new(2021, 3, 2, 9, 00, 0);
                Parcels[2].CreatParcel = new(2021, 3, 2, 8, 30, 0);
                Parcels[3].CreatParcel = new(2021, 3, 2, 6, 30, 0);
                Parcels[4].CreatParcel = new(2021, 3, 2, 9, 30, 0);
                Parcels[5].CreatParcel = new(2021, 3, 2, 4, 30, 0);
                Parcels[6].CreatParcel = new(2021, 3, 2, 5, 30, 0);
                Parcels[7].CreatParcel = new(2021, 3, 2, 8, 30, 0);
                Parcels[8].CreatParcel = new(2021, 3, 2, 6, 30, 0);
                Parcels[9].CreatParcel = new(2021, 3, 2, 1, 30, 0);

                Parcels[0].Delivered = new(2021, 3,5, 8, 30, 0);
                Parcels[1].Delivered = new(2021, 6,5, 9, 00, 0);
                Parcels[2].Delivered = new(2021, 3,5, 8, 30, 0);
                Parcels[3].Delivered = new(2021, 3,5, 9, 30, 0);
                Parcels[4].Delivered = new(2021, 3,5, 2, 30, 0);
                Parcels[5].Delivered = new(2021, 3,5, 13, 30, 0);
                Parcels[6].Delivered = new(2021, 3,5, 18, 30, 0);
                Parcels[7].Delivered = new(2021, 3,5, 21, 30, 0);
                Parcels[8].Delivered = new(2021, 3,5, 20, 30, 0);
                Parcels[9].Delivered = new(2021, 3,5, 23, 30, 0);

                Parcels[0].PickedUp = new(2021, 3, 1, 8, 30, 0);
                Parcels[1].PickedUp = new(2021, 6, 2, 9, 00, 0);
                Parcels[2].PickedUp = new(2021, 3, 3, 8, 30, 0);
                Parcels[3].PickedUp = new(2021, 3, 4, 8, 30, 0);
                Parcels[4].PickedUp = new(2021, 3, 5, 8, 30, 0);
                Parcels[5].PickedUp = new(2021, 3, 6, 8, 30, 0);
                Parcels[6].PickedUp = new(2021, 3, 7, 8, 30, 0);
                Parcels[7].PickedUp = new(2021, 3, 8, 8, 30, 0);
                Parcels[8].PickedUp = new(2021, 3, 21, 8, 30, 0);
                Parcels[9].PickedUp = new(2021, 3, 1, 8, 30, 0);

                Parcels[0].Scheduled = new(2021, 3, 1, 10, 30, 0);
                Parcels[1].Scheduled = new(2021, 6, 2, 10, 00, 0);
                Parcels[2].Scheduled = new(2021, 3, 3, 10, 30, 0);
                Parcels[3].Scheduled = new(2021, 3, 4, 10, 30, 0);
                Parcels[4].Scheduled = new(2021, 3, 5, 10, 30, 0);
                Parcels[5].Scheduled = new(2021, 3, 6, 10, 30, 0);
                Parcels[6].Scheduled = new(2021, 3, 7, 10, 30, 0);
                Parcels[7].Scheduled = new(2021, 3, 8, 10, 30, 0);
                Parcels[8].Scheduled = new(2021, 3, 9, 10, 30, 0);
                Parcels[9].Scheduled = new(2021, 3, 10, 10, 30, 0);
            }
        }
    }
}

