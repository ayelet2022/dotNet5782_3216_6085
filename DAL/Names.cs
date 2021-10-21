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
                        DataSource.Drones[DataSource.Config.DronesIndex].Battery= Rand.Next(0, 101);
                        DataSource.Drones[DataSource.Config.DronesIndex].Model = "xxx";
                        DataSource.Config.DronesIndex++;
                        break;
                    case "Customer":
                        Console.WriteLine("Enter Id: ");
                        DataSource.Customers[DataSource.Config.CustomersIndex].Id = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Latitude: ");
                        DataSource.Customers[DataSource.Config.CustomersIndex].Latitude = double.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Longitude: ");
                        DataSource.Customers[DataSource.Config.CustomersIndex].Longitude= double.Parse(Console.ReadLine());
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
          
                        DataSource.Parcels[DataSource.Config.ParcelsIndex].DroneId =DronToAParcel(DataSource.Parcels[DataSource.Config.ParcelsIndex]);
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

            int DronToAParcel(Parcel newParcel)
            {
                int i = 0;
                while ((DataSource.Drones[i].Status != (DroneStatuses)0) && (DataSource.Drones[i].Battery != 0) &&
                    (DataSource.Drones[i].MaxWeight >= newParcel.Weight))
                    i++;
                DataSource.Drones[i].Status = (DroneStatuses)2;
                return DataSource.Drones[i].Id;
            }

            void DronToCharger(Drone dronToCharge )
            {
                Console.WriteLine("Enter the name of the basestation you whant to charge the drone in: ");
                string NameOfBaseStation = Console.ReadLine();
                int i = 0;
                while (i < DataSource.Config.StationsIndex && DataSource.Stations[i].Name != NameOfBaseStation)
                    i++;
                DataSource.Stations[i].EmptyCharges--;
                dronToCharge.Status = (DroneStatuses)1;
            }

            void FreeDroneFromBaseStation(Drone dronToFree)
            {
                dronToFree.Battery = 100;
                dronToFree.Status= (DroneStatuses)0;
            }

            /// <summary>
            /// parcel was picked up by the dron
            /// </summary>
            /// <param name="ParclToPickup"></param> the parcel that needs delevering
            void PickUpParcel(Parcel ParclToPickup)
            {
                ParclToPickup.PickedUp= DateTime.Now;
            }

            
            void ParcelToCustomer(Parcel ParcelDeliverd )
            {
                ParcelDeliverd.Delivered = DateTime.Now;
                int i=0;
                while (i < DataSource.Config.DronesIndex && DataSource.Drones[i].Id != ParcelDeliverd.DroneId)
                    i++;
                DataSource.Drones[i].Status = (DroneStatuses)0;
            }

            }

        }
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

