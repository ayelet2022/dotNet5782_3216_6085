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
        static Random Rand = new Random();
        /// <summary>
        /// restarting the indexes
        /// </summary>
        internal class Config
        {
            internal static double Available = 0.1;
            internal static double Light = 1;
            internal static double MediumWeight = 2;
            internal static double Heavy = 4;
            internal static int RunningParcelId = 1000;
            internal static double ChargingRate =50;
        }
        static void InitializeBaseStation(string name)
        {
            BaseStation newStation = new BaseStation();
            newStation.Id = Rand.Next(1000, 10000);
            newStation.Latitude = Rand.Next(30, 34);
            newStation.Longitude = Rand.Next(34,38);
            newStation.EmptyCharges = Rand.Next(0, 6);
            newStation.Name = name;
            Stations.Add(newStation);
        }
        static void InitializeDrones(string name)
        {
            Drone addDrone = new Drone();
            addDrone.Id = Rand.Next(100000, 999999);
            addDrone.MaxWeight = (WeightCategories)Rand.Next(0, 3);
            addDrone.Model = name;
            Drones.Add(addDrone);
        }
        static void InitializeCustomer(string phone,string name)
        {
            Customer addCustomer = new Customer();
            addCustomer.Id = Rand.Next(100000000, 1000000000);
            addCustomer.Latitude = Rand.Next(30, 34);
            addCustomer.Longitude = Rand.Next(34, 38);
            addCustomer.Phone = phone;
            addCustomer.Name = name;
            Customers.Add(addCustomer);
        }
        static void InitializeParcel(DateTime timeCreatParcel)
        {
            Parcel addParcel = new Parcel();
            addParcel.CreatParcel = new(0);
            addParcel.Id = DataSource.Config.RunningParcelId++;
            addParcel.Priority = (Priorities)Rand.Next(0, 3);
            int senderId = Rand.Next(0, Customers.Count);
            addParcel.SenderId = Customers[senderId].Id;
            int targetId = Rand.Next(0, Customers.Count);
            addParcel.TargetId = Customers[targetId].Id;
            addParcel.Weight = (WeightCategories)Rand.Next(0, 3);
            addParcel.DroneId = 0;
            addParcel.Delivered = DateTime.MinValue;
            addParcel.PickedUp = DateTime.MinValue;
            addParcel.Scheduled = DateTime.MinValue;
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

            InitializeParcel(new(2021, 3, 5, 8, 30, 0));
            InitializeParcel(new(2021, 3, 5, 9, 30, 0));
            InitializeParcel(new(2021, 3, 5, 10, 30, 0));
            InitializeParcel(new(2021, 3, 5, 11, 30, 0));
            InitializeParcel(new(2021, 3, 5, 12, 30, 0));
            InitializeParcel(new(2021, 3, 5, 13, 30, 0));
            InitializeParcel(new(2021, 3, 5, 14, 30, 0));
            InitializeParcel(new(2021, 3, 5, 15, 30, 0));
            InitializeParcel(new(2021, 3, 5, 16, 30, 0));
            InitializeParcel(new(2021, 3, 5, 17, 30, 0));
        }
    }
}
