using System;
using IDAL.DO;
using System.Collections.Generic;
namespace DalObject
{
    /// <summary>
    /// includs all the data of the object-the arr of each object,
    /// and the index of each arr 
    /// </summary>
    public static class DataSource
    {
        internal static List<Drone> Drones = new List<Drone>();
        internal static List<BaseStation> Stations = new List<BaseStation>();
        internal static List<Customer> Customers = new List<Customer>();
        internal static List<Parcel> Parcels = new List<Parcel>();
        internal static List<DroneCharge> DroneCharges = new List<DroneCharge>();
        /// <summary>
        /// restarting the indexes
        /// </summary>
        internal class Config
        {
            internal static double Available = 0.001;
            internal static double Light = 0.01;
            internal static double MediumWeight = 0.02;
            internal static double Heavy = 0.04;
            internal static int RunningParcelId = 1000;
            internal static double ChargingRate = 0.5;
        }
        static void InitializeBaseStation(string name)
        {
            Random Rand = new Random(DateTime.Now.Millisecond);
            BaseStation newStation = new BaseStation();
            newStation.Id = Rand.Next(1000, 10000);
            newStation.Latitude = Rand.Next(-90, 91);
            newStation.Longitude = Rand.Next(-180, 181);
            newStation.EmptyCharges = Rand.Next(0, 6);
            newStation.Name = name;
            Stations.Add(newStation);
        }
        static void InitializeDrones(string name)
        {
            Random Rand = new Random(DateTime.Now.Millisecond);
            Drone addDrone = new Drone();
            addDrone.Id = Rand.Next(100000, 999999);
            addDrone.MaxWeight = (WeightCategories)Rand.Next(0, 3);
            //addDrone.Status = (DroneStatuses)Rand.Next(0, 3);
            addDrone.Model = name;
            Drones.Add(addDrone);
        }
        static void InitializeCustomer(string phone,string name)
        {
            Random Rand = new Random(DateTime.Now.Millisecond);
            Customer addCustomer = new Customer();
            addCustomer.Id = Rand.Next(100000000, 1000000000);
            addCustomer.Latitude = Rand.Next(30, 33);
            addCustomer.Longitude = Rand.Next(34, 37);
            addCustomer.Phone = phone;
            addCustomer.Name = name;
            Customers.Add(addCustomer);
        }
        static void InitializeParcel(int droneId,DateTime timeCreatParcel,DateTime timeScheduled,DateTime timePickedUp,DateTime timeDelivered)
        {
            Random Rand = new Random(DateTime.Now.Millisecond);
            Parcel addParcel = new Parcel();
            addParcel.CreatParcel = new(0);
            addParcel.Id = DataSource.Config.RunningParcelId++;
            addParcel.Priority = (Priorities)Rand.Next(0, 3);
            int senderId = Rand.Next(0, Customers.Count);
            addParcel.SenderId = Customers[senderId].Id;
            int targetId = Rand.Next(0, Customers.Count);
            addParcel.TargetId = Customers[targetId].Id;
            addParcel.Weight = (WeightCategories)Rand.Next(0, 3);
            addParcel.DroneId = droneId;
            addParcel.Delivered = timeDelivered;
            addParcel.PickedUp = timePickedUp;
            addParcel.Scheduled = timeScheduled;
            addParcel.CreatParcel = timeCreatParcel;
            Parcels.Add(addParcel);
        }

        /// <summary>
        /// Incluods the data that we enterd
        /// </summary>
        internal static void Initialize()
        {
            InitializeBaseStation("Banana");
            InitializeBaseStation("Apple");

            InitializeDrones("Ab89ZX");
            InitializeDrones("Ui65JH");
            InitializeDrones("Gy70CW");
            InitializeDrones("Qs98VM");
            InitializeDrones("Aa44ZX");

            InitializeCustomer("0511111111", "Ayelet");
            InitializeCustomer("0522222222", "Penina");
            InitializeCustomer("0533333333", "Yosi");
            InitializeCustomer("0544444444", "Avi");
            InitializeCustomer("0555555555", "Nomi");
            InitializeCustomer("0566666666", "Michal");
            InitializeCustomer("0577777777", "Daniel");
            InitializeCustomer("0588888888", "Chaya");
            InitializeCustomer("0599999999", "Chani");
            InitializeCustomer("0500000000", "Yakov");

            InitializeParcel(Drones[0].Id,new(2021, 3, 5, 8, 30, 0), new(2021, 3, 1, 8, 32, 0), new(2021, 3, 1, 9, 30, 0), new(2021, 3, 2, 10, 30, 0));
            InitializeParcel(0,new(2021, 3, 5, 9, 30, 0), new(2021, 3, 1, 9, 32, 0), new(2021, 3, 1, 10, 30, 0), new(2021, 3, 2, 11, 30, 0));
            InitializeParcel(0,new(2021, 3, 5, 10, 30, 0), new(2021, 3, 1, 10, 32, 0), new(2021, 3, 1, 11, 30, 0), new(2021, 3, 2, 12, 30, 0));
            InitializeParcel(Drones[2].Id, new(2021, 3, 5, 11, 30, 0), new(2021, 3, 1, 11, 32, 0), new(2021, 3, 1, 12, 30, 0), new(2021, 3, 2, 13, 30, 0));
            InitializeParcel(0,new(2021, 3, 5, 12, 30, 0), new(2021, 3, 1, 12, 32, 0), new(2021, 3, 1, 13, 30, 0), new(2021, 3, 2, 14, 30, 0));
            InitializeParcel(0,new(2021, 3, 5, 13, 30, 0), new(2021, 3, 1, 13, 32, 0), new(2021, 3, 1, 14, 30, 0), new(2021, 3, 2, 15, 30, 0));
            InitializeParcel(0,new(2021, 3, 5, 14, 30, 0), new(2021, 3, 1, 14, 32, 0), new(2021, 3, 1, 15, 30, 0), new(2021, 3, 2, 16, 30, 0));
            InitializeParcel(0,new(2021, 3, 5, 15, 30, 0), new(2021, 3, 1, 15, 32, 0), new(2021, 3, 1, 16, 30, 0), new(2021, 3, 2, 17, 30, 0));
            InitializeParcel(0,new(2021, 3, 5, 16, 30, 0), new(2021, 3, 1, 16, 32, 0), new(2021, 3, 1, 17, 30, 0), new(2021, 3, 2, 18, 30, 0));
            InitializeParcel(0,new(2021, 3, 5, 17, 30, 0), new(2021, 3, 1, 17, 32, 0), new(2021, 3, 1, 18, 30, 0), new(2021, 3, 2, 19, 30, 0));
        }
    }
}
