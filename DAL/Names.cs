using System;
using DAL.IDAL.DO;
namespace DAL
{
    namespace DalObject
    {
        public class DalObject
        {
            Random Rand = new Random(DateTime.Now.Millisecond);
            DalObject()
            {
                DataSource.Initialize();
            }
            /// <summary>
            /// adds a new object(a new drone\a new basestation\a new customer\a new parcel) 
            /// and updates all that needs to be updated becuas of what we added
            /// </summary>
            internal void AddObject()
            {
                string Choice = Console.ReadLine();
                switch (Choice)
                {
                    case "BaseStation":
                        DataSource.Stations[DataSource.Config.StationsIndex].Id = Rand.Next(0, 1000000);
                        DataSource.Stations[DataSource.Config.StationsIndex].Latitude = Rand.Next(0, 1000000);
                        DataSource.Stations[DataSource.Config.StationsIndex].Latitude = Rand.Next(0, 1000000);
                        DataSource.Stations[DataSource.Config.StationsIndex].EmptyCharges = Rand.Next(0, 15);
                        DataSource.Stations[DataSource.Config.StationsIndex].Name = "aaa";
                        DataSource.Config.StationsIndex++;
                        break;

                    case "Drone":
                        DataSource.Drones[DataSource.Config.DronesIndex].Id = Rand.Next(0, 1000000);
                        DataSource.Drones[DataSource.Config.DronesIndex].MaxWeight = (WeightCategories)Rand.Next(0, 3);
                        DataSource.Drones[DataSource.Config.DronesIndex].Status = (DroneStatuses)Rand.Next(0, 3);
                        DataSource.Drones[DataSource.Config.DronesIndex].Battery = Rand.Next(0, 101);
                        DataSource.Drones[DataSource.Config.DronesIndex].Model = "xxx";
                        DataSource.Config.DronesIndex++;
                        break;

                    case "Customer":
                        Console.WriteLine("Enter Id: ");
                        DataSource.Customers[DataSource.Config.CustomersIndex].Id = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Latitude: ");
                        DataSource.Customers[DataSource.Config.CustomersIndex].Latitude = double.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Longitude: ");
                        DataSource.Customers[DataSource.Config.CustomersIndex].Longitude = double.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Name: ");
                        DataSource.Customers[DataSource.Config.CustomersIndex].Name = Console.ReadLine();
                        Console.WriteLine("Enter Phone: ");
                        DataSource.Customers[DataSource.Config.CustomersIndex].Phone = Console.ReadLine();
                        DataSource.Config.CustomersIndex++;
                        break;

                    case "Parcel":
                        DataSource.Parcels[DataSource.Config.ParcelsIndex].CreatParcel = DateTime.Now;
                        Console.WriteLine("Enter Parcel Id: ");
                        DataSource.Parcels[DataSource.Config.ParcelsIndex].Id = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter SenderId: ");
                        DataSource.Parcels[DataSource.Config.ParcelsIndex].SenderId = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter TargetId: ");
                        DataSource.Parcels[DataSource.Config.ParcelsIndex].TargetId = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Weight (light/inbetween/heavy): ");
                        string Weight = Console.ReadLine();
                        if (Weight == "light")
                            DataSource.Parcels[DataSource.Config.ParcelsIndex].Weight = (WeightCategories)0;
                        if (Weight == "inbetween")
                            DataSource.Parcels[DataSource.Config.ParcelsIndex].Weight = (WeightCategories)1;
                        if (Weight == "heavy")
                            DataSource.Parcels[DataSource.Config.ParcelsIndex].Weight = (WeightCategories)2;
                        DataSource.Parcels[DataSource.Config.ParcelsIndex].Scheduled = DateTime.Now;
                        DataSource.Parcels[DataSource.Config.ParcelsIndex].DroneId = DronToAParcel(DataSource.Parcels[DataSource.Config.ParcelsIndex]);
                        Console.WriteLine("Enter Priority (regular/fast/urgent): ");
                        string Priority = Console.ReadLine();
                        if (Priority == "regular")
                            DataSource.Parcels[DataSource.Config.ParcelsIndex].Priority = (Priorities)0;
                        if (Priority == "fast")
                            DataSource.Parcels[DataSource.Config.ParcelsIndex].Priority = (Priorities)1;
                        if (Priority == "urgent")
                            DataSource.Parcels[DataSource.Config.ParcelsIndex].Priority = (Priorities)2;
                        PickUpParcel(DataSource.Parcels[DataSource.Config.ParcelsIndex]);
                        ParcelToCustomer(DataSource.Parcels[DataSource.Config.ParcelsIndex]);
                        break;
                }
            }

            /// <summary>
            /// looks for a free drone that can the delever the parcel to the customer
            /// </summary>
            /// <param name="newParcel"></param> the parcel thats the dron need to deliver
            /// <returns></returns>the id of the drone that can deliver the parcel
            public static void DronToAParcel(int NewId)
            {
                int j = 0;
                while (DataSource.Parcels[j].Id != NewId)
                    j++;
                int i = 0;
                while ((DataSource.Drones[i].Status != (DroneStatuses)0) && (DataSource.Drones[i].Battery != 0) &&
                    (DataSource.Drones[i].MaxWeight >= DataSource.Parcels[j].Weight))
                    i++;
                DataSource.Drones[i].Status = (DroneStatuses)2;
                //return DataSource.Drones[i].Id;
            }

            /// <summary>
            /// looks for an empty charger in the base station so the drone will be able to charge there 
            /// and updates all that needs to be update becaus of that
            /// </summary>
            /// <param name="dronToCharge"></param> the drone that need charging
            public static void DronToCharger(int DronesId, string NameOfBaseStation)
            {
                int j = 0;
                while (DataSource.Drones[j].Id != DronesId)
                    j++;
                int i = 0;
                while (i < DataSource.Config.StationsIndex && DataSource.Stations[i].Name != NameOfBaseStation)
                    i++;
                DataSource.Stations[i].EmptyCharges--;
                DataSource.Drones[j].Status = (DroneStatuses)1;
            }

            /// <summary>
            /// free the drone from the charger that he was cherging from in the base station
            /// </summary>
            /// <param name="dronToFree"></param>the drone that needs to get free from the charger he his charging from
            public static void FreeDroneFromBaseStation(int DronesId)
            {
                int j = 0;
                while (DataSource.Drones[j].Id != DronesId)
                    j++;
                DataSource.Drones[j].Battery = 100;
                DataSource.Drones[j].Status = (DroneStatuses)0;
            }

            /// <summary>
            /// updates the time that the parcel was picked up by the dron
            /// </summary>
            /// <param name="ParclToPickup"></param> the parcel that needs delevering
            public static void PickUpParcel(int NewId)
            {
                int j = 0;
                while (DataSource.Parcels[j].Id != NewId)
                    j++;
                DataSource.Parcels[j].PickedUp = DateTime.Now;
            }

            /// <summary>
            /// updates the time that the parcel was deleverd to the customer
            /// </summary>
            /// <param name="ParcelDeliverd"></param>
            public static void ParcelToCustomer(int NewId)
            {
                int j = 0;
                while (DataSource.Parcels[j].Id != NewId)
                    j++;
                DataSource.Parcels[j].Delivered = DateTime.Now;
                int i = 0;
                while (i < DataSource.Config.DronesIndex && DataSource.Drones[i].Id != DataSource.Parcels[j].DroneId)
                    i++;
                DataSource.Drones[i].Status = (DroneStatuses)0;
            }

            /// <summary>
            /// prints the object(base station,drone,customer,parcel) 
            /// that the user wants to print according to the object id
            /// </summary>
            void PrintObject()
            {
                Console.WriteLine("Enter What Would Yot Want To Print: ");
                string Choice = Console.ReadLine();
                int i = 0;
                switch (Choice)
                {
                    case "BaseStation":
                        Console.WriteLine("Enter the BaseStation Id: ");
                        int BaseStationId = int.Parse(Console.ReadLine());
                        while (DataSource.Stations[i].Id != BaseStationId)
                            i++;
                        DataSource.Stations[i].ToString();
                        break;
                    case "Drone":
                        Console.WriteLine("Enter the Drone Id: ");
                        int DroneId = int.Parse(Console.ReadLine());
                        while (DataSource.Drones[i].Id != DroneId)
                            i++;
                        DataSource.Drones[i].ToString();
                        break;
                    case "Customer":
                        Console.WriteLine("Enter the Customer Id: ");
                        int CustomerId = int.Parse(Console.ReadLine());
                        while (DataSource.Customers[i].Id != CustomerId)
                            i++;
                        DataSource.Customers[i].ToString();
                        break;
                    case "Parcel":
                        Console.WriteLine("Enter the Parcel Id: ");
                        int ParcelId = int.Parse(Console.ReadLine());
                        while (DataSource.Parcels[i].Id != ParcelId)
                            i++;
                        DataSource.Parcels[i].ToString();
                        break;
                }
            }

            /// <summary>
            /// prints the whole list of the object according to what the user enterd
            /// </summary>
            public static BaseStation[] PrintBaseStations()
            {
                BaseStation[] ActiveStations = new BaseStation[DataSource.Config.StationsIndex]; 
                for (int i = 0; i < DataSource.Config.StationsIndex; i++)
                {
                    ActiveStations[i] = DataSource.Stations[i];
                    //ActiveStations[i].Id = DataSource.Stations[i].Id;
                    //ActiveStations[i].Name = DataSource.Stations[i].Name;
                    //ActiveStations[i].EmptyCharges = DataSource.Stations[i].EmptyCharges;
                    //ActiveStations[i].Latitude = DataSource.Stations[i].Latitude;
                    //ActiveStations[i].Longitude = DataSource.Stations[i].Longitude;
                }
                return ActiveStations;
            }
            public static Drone[] PrintDrones()
            {
                Drone[] ActiveDrones = new Drone[DataSource.Config.DronesIndex];
                for (int i = 0; i < DataSource.Config.DronesIndex; i++)
                    ActiveDrones[i] = DataSource.Drones[i];
                return ActiveDrones;
            }
            public static Customer[] PrintCustomers()
            {
                Customer[] ActiveCustomers = new Customer[DataSource.Config.CustomersIndex];
                for (int i = 0; i < DataSource.Config.CustomersIndex; i++)
                    ActiveCustomers[i] = DataSource.Customers[i];
                return ActiveCustomers;
            }
            public static Parcel[] PrintPercels()
            {
                Parcel[] ActiveParcels = new Parcel[DataSource.Config.ParcelsIndex];
                for (int i = 0; i < DataSource.Config.ParcelsIndex; i++)
                    ActiveParcels[i] = DataSource.Parcels[i];
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
                        Parcels[i] = DataSource.Parcels[i];
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
                        Stations[i] = DataSource.Stations[i];
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
            internal class Config
            {
                internal static int DronesIndex = 0;
                internal static int StationsIndex = 0;
                internal static int CustomersIndex = 0;
                internal static int ParcelsIndex = 0;
                internal static int RunningParcelId = 0;
            }
            internal static void Initialize()
            {
                Random Rand = new Random(DateTime.Now.Millisecond);
                for (int i = 0; i < 2; i++)
                {
                    Stations[i].Id = Rand.Next(0, 1000000);
                    Stations[i].Latitude = Rand.Next(0, 1000000);
                    Stations[i].Latitude = Rand.Next(0, 1000000);
                    Stations[i].EmptyCharges = Rand.Next(0,15);
                    DataSource.Config.StationsIndex++;
                }
                Stations[0].Name = "aaa";
                Stations[1].Name = "bbb";

                for (int i = 0; i < 5; i++)
                {
                    Drones[i].Id = Rand.Next(0, 1000000);
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
                    Customers[i].Id = Rand.Next(0, 1000000);
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

                Customers[0].Phone = "051111111";
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
                    Parcels[i].CreatParcel = DateTime.Now;
                    Parcels[i].Id = DataSource.Config.RunningParcelId++;
                    Parcels[i].Priority = (Priorities)Rand.Next(0,3);
                    Parcels[i].SenderId= Rand.Next(0, 1000000);
                    Parcels[i].TargetId = Rand.Next(0, 1000000);
                    Parcels[i].Weight = (WeightCategories)Rand.Next(0, 3);
                    int j = 0;
                    while ((DataSource.Drones[j].Status != (DroneStatuses)0) && (DataSource.Drones[j].Battery != 0) &&
                        (DataSource.Drones[j].MaxWeight >= DataSource.Parcels[DataSource.Config.ParcelsIndex].Weight))
                        j++;
                    Parcels[i].DroneId = DataSource.Drones[j].Id;
                    Parcels[i].Scheduled = DateTime.Now;
                    Parcels[i].PickedUp = DateTime.Now;
                    Parcels[i].Delivered = DateTime.Now;
                    DataSource.Config.ParcelsIndex++;
                }
            }
        }
    }
}

