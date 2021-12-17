using DO;
using System;
using System.Collections.Generic;

namespace Dal
{
    /// <summary>
    /// includs all the data of the object-the arr of each object,
    /// and the index of each arr 
    /// </summary>
    internal static class DataSource
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
            internal static double ChargingRate =200;
        }
        static void InitializeBaseStation(string name)
        {
            BaseStation newStation = new BaseStation();
            newStation.Id = Rand.Next(1000, 10000);
            newStation.Latitude = Rand.Next(30, 34) + Rand.NextDouble(); ;
            newStation.Longitude = Rand.Next(34,38) + Rand.NextDouble(); ;
            newStation.EmptyCharges = Rand.Next(0, 6);
            newStation.Name = name;
            Stations.Add(newStation);
        }
        static void InitializeDrones(string name, int weight)
        {
            Drone addDrone = new Drone();
            addDrone.Id = Rand.Next(100000, 999999);
            addDrone.MaxWeight = (WeightCategories)weight;
            addDrone.Model = name;
            Drones.Add(addDrone);
        }
        static void InitializeCustomer(string phone,string name)
        {
            Customer addCustomer = new Customer();
            addCustomer.Id = Rand.Next(100000000, 1000000000);
            addCustomer.Latitude = Rand.Next(30, 34) + Rand.NextDouble(); ;
            addCustomer.Longitude = Rand.Next(34, 38) + Rand.NextDouble(); ;
            addCustomer.Phone = phone;
            addCustomer.Name = name;
            Customers.Add(addCustomer);
        }
        static void InitializeParcel()
        {
            for (int index = 0; index < 10; index++)//Updating 10 parcels
            {
                Parcel newParcel = new();
                newParcel.Id = Config.RunningParcelId++;//Updating the ID number of the package
                newParcel.SenderId = Customers[Rand.Next(10)].Id;//Updating the ID number of the sender
                newParcel.DroneId = 0;//Updating the ID number of the drone
                do
                {
                    newParcel.TargetId = Customers[Rand.Next(10)].Id;
                }
                while (newParcel.SenderId == newParcel.TargetId);

                newParcel.Weight = (WeightCategories)Rand.Next(0,3);//Updating the weight
                newParcel.Priority = (Priorities)Rand.Next(0,3);//Updating the urgency of the shipment
                //Putting a Random date and time
                newParcel.CreatParcel = new DateTime(2021, Rand.Next(1, 13), Rand.Next(1, 29),
                    Rand.Next(24), Rand.Next(60), Rand.Next(60));
                int status = Rand.Next(100);
                int drone = -1;
                if (status >= 10)
                {
                    //Scheduling a time to deliver parcel
                    newParcel.Scheduled = newParcel.CreatParcel +
                        new TimeSpan(Rand.Next(5), Rand.Next(60), Rand.Next(60));
                    if (status >= 15)
                    {
                        //Time drone came to deliver parcel
                        newParcel.PickedUp = newParcel.Scheduled +
                            new TimeSpan(0, Rand.Next(1, 60), Rand.Next(60));
                        if (status >= 20)
                        {
                            //Time customer recieved parcel
                            newParcel.Delivered = newParcel.PickedUp +
                                new TimeSpan(0, Rand.Next(1, 60), Rand.Next(60));
                            do
                            {
                                drone = Rand.Next(5);
                                newParcel.DroneId = Drones[drone].Id;
                            }
                            while (Drones[drone].MaxWeight < newParcel.Weight);
                        }
                    }
                    if (drone == -1)
                    {
                        do
                        {
                            drone = Rand.Next(5);
                            newParcel.DroneId = Drones[drone].Id;
                        }
                        while (Drones[drone].MaxWeight > newParcel.Weight);
                    }
                }
                Parcels.Add(newParcel);
            }
        }

        /// <summary>
        /// Incluods the data that we enterd
        /// </summary>
        internal static void Initialize()
        {
            InitializeBaseStation("Banana");
            InitializeBaseStation("Apple");

            InitializeDrones("Ab89ZX", 2);
            InitializeDrones("Ui65JH", 2);
            InitializeDrones("Gy70CW", 2);
            InitializeDrones("Qs98VM", 1);
            InitializeDrones("Aa44ZX", 0);

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

            InitializeParcel();

        }
    }
}
