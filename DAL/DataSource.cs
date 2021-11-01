using System;
using IDAL.DO;
namespace DalObject
{
    /// <summary>
    /// includs all the data of the object-the arr of each object,
    /// and the index of each arr 
    /// </summary>
    public static class DataSource
    {
        internal static Drone[] Drones = new Drone[10];
        internal static BaseStation[] Stations = new BaseStation[5];
        internal static Customer[] Customers = new Customer[100];
        internal static Parcel[] Parcels = new Parcel[1000];
        internal static DroneCharge[] DroneCharges = new DroneCharge[25];
        /// <summary>
        /// restarting the indexes
        /// </summary>
        internal class Config
        {
            internal static int DronesIndex = 0;
            internal static int StationsIndex = 0;
            internal static int CustomersIndex = 0;
            internal static int ParcelsIndex = 0;
            internal static int DroneChargesIndex = 0;
            internal static int RunningParcelId = 1000;
        }
          static DataSource()=>Initialize();

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
                Stations[i].EmptyCharges = Rand.Next(0, 6);
                DataSource.Config.StationsIndex++;
            }
            Stations[0].Name = "aaa";
            Stations[1].Name = "bbb";

            for (int i = 0; i < 5; i++)
            {
                Drones[i].Id = Rand.Next(1000, 10000);
                Drones[i].MaxWeight = (WeightCategories)Rand.Next(0, 3);
                Drones[i].Status = (DroneStatuses)Rand.Next(0, 3);
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
                Parcels[i].Priority = (Priorities)Rand.Next(0, 3);
                Parcels[i].SenderId = Rand.Next(1000, 10000);
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

            Parcels[0].Delivered = new(2021, 3, 5, 8, 30, 0);
            Parcels[1].Delivered = new(2021, 6, 5, 9, 00, 0);
            Parcels[2].Delivered = new(2021, 3, 5, 8, 30, 0);
            Parcels[3].Delivered = new(2021, 3, 5, 9, 30, 0);
            Parcels[4].Delivered = new(2021, 3, 5, 2, 30, 0);
            Parcels[5].Delivered = new(2021, 3, 5, 13, 30, 0);
            Parcels[6].Delivered = new(2021, 3, 5, 18, 30, 0);
            Parcels[7].Delivered = new(2021, 3, 5, 21, 30, 0);
            Parcels[8].Delivered = new(2021, 3, 5, 20, 30, 0);
            Parcels[9].Delivered = new(2021, 3, 5, 23, 30, 0);

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
