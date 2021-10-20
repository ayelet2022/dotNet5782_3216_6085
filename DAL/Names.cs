﻿using System;
using DAL.IDAL.DO;
namespace DAL
{
    namespace DalObject
    {
        public class DalObject
        {
            DalObject()
            {
                DataSource.Initialize();
            }
        }
        class DataSource
        {
            static Drone[] Drones = new Drone[10];
            static BaseStation[] Stations = new BaseStation[5];
            static Customer[] Customers = new Customer[100];
            static Parcel[] Parcels = new Parcel[1000];
            internal class Config
            {
                internal static int DronesIndex = 0;
                internal static int StationsIndex = 0;
                internal static int CustomersIndex = 0;
                internal static int ParcelsIndex = 0;
                int I = 0;
            }
            public static void Initialize()
            {
                Random Rand = new Random(DateTime.Now.Millisecond);
                for (int i = 0; i < 2; i++)
                {
                    Stations[i].Id = Rand.Next(0, 1000000);
                    Stations[i].Latitude = Rand.Next(0, 1000000);
                    Stations[i].Latitude = Rand.Next(0, 1000000);
                    Stations[i].EmptyCharges = Rand.Next(0, 10);
                    DataSource.Config.StationsIndex++;
                }
                Stations[0].Name = "aaa";
                Stations[1].Name = "bbb";

                for (int i = 0; i < 5; i++)
                {
                    Drones[i].Id = Rand.Next(0, 1000000);
                    Drones[i].MaxWeight = (WeightCategories)Rand.Next(0,2);
                    Drones[i].Status = (DroneStatuses)Rand.Next(0, 2);
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
            }
        }

        }
    }
}
