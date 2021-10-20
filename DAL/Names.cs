using System;
using DAL.IDAL.DO;

Random Rand = new Random(DateTime.Now.Millisecond);

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
                static int DronesIndex = 0;
                static int StationsIndex = 0;
                static int CustomersIndex = 0;
                static int ParcelsIndex = 0;
                int I = 0;
            }
            public static void Initialize()
            {
                for(int i=0;i<2;i++)
                {
                    Stations[i].Id = Rand.Next(0,1000000);

                }









                drones[DataSource.Config.dronesIndex].battery = rand.Next(0, 100);
                drones[DataSource.Config.dronesIndex].id = 1111;
                drones[DataSource.Config.dronesIndex].maxWeight = (WeightCategories)0;
                drones[DataSource.Config.dronesIndex].model = "aaa";
                drones[DataSource.Config.dronesIndex].status = (DroneStatuses)0;
                DataSource.Config.dronesIndex++;
                drones[DataSource.Config.dronesIndex].battery = 90;
                drones[DataSource.Config.dronesIndex].id = 2222;
                drones[DataSource.Config.dronesIndex].maxWeight = (WeightCategories)1;
                drones[DataSource.Config.dronesIndex].model = "bbb";
                drones[DataSource.Config.dronesIndex].status = (DroneStatuses)1;
                DataSource.Config.dronesIndex++;
                drones[DataSource.Config.dronesIndex].battery = 80;
                drones[DataSource.Config.dronesIndex].id = 3333;
                drones[DataSource.Config.dronesIndex].maxWeight = (WeightCategories)2;
                drones[DataSource.Config.dronesIndex].model = "ccc";
                drones[DataSource.Config.dronesIndex].status = (DroneStatuses)2;
                DataSource.Config.dronesIndex++;
                drones[DataSource.Config.dronesIndex].battery = 70;
                drones[DataSource.Config.dronesIndex].id = 4444;
                drones[DataSource.Config.dronesIndex].maxWeight = (WeightCategories)0;
                drones[DataSource.Config.dronesIndex].model = "ddd";
                drones[DataSource.Config.dronesIndex].status = (DroneStatuses)1;
                DataSource.Config.dronesIndex++;
                drones[DataSource.Config.dronesIndex].battery = 60;
                drones[DataSource.Config.dronesIndex].id = 4444;
                drones[DataSource.Config.dronesIndex].maxWeight = (WeightCategories)1;
                drones[DataSource.Config.dronesIndex].model = "yyy";
                drones[DataSource.Config.dronesIndex].status = (DroneStatuses)1;
                DataSource.Config.dronesIndex++;

                stations[DataSource.Config.stationsIndex].emptyCharges = 4;
                stations[DataSource.Config.stationsIndex].id = 264;
                stations[DataSource.Config.stationsIndex].latitude = 26.8;
                stations[DataSource.Config.stationsIndex].longitud = 50;
                stations[DataSource.Config.stationsIndex].name = "jerusalem";
                DataSource.Config.stationsIndex++;
                stations[DataSource.Config.stationsIndex].emptyCharges = 4;
                stations[DataSource.Config.stationsIndex].id = 264;
                stations[DataSource.Config.stationsIndex].latitude = 26.8;
                stations[DataSource.Config.stationsIndex].longitud = 50;
                stations[DataSource.Config.stationsIndex].name = "jerusalem";
                DataSource.Config.stationsIndex++;

                customers[DataSource.Config.customersIndex].id = 111111111;
                customers[DataSource.Config.customersIndex].latitude = 111111111;
                customers[DataSource.Config.customersIndex].longitud = 111111111;
                customers[DataSource.Config.customersIndex].name = "Ayelet";
                customers[DataSource.Config.customersIndex].phone = "0548735177";
                DataSource.Config.customersIndex++;
                customers[DataSource.Config.customersIndex].id = 222222222;
                customers[DataSource.Config.customersIndex].latitude = 111111111;
                customers[DataSource.Config.customersIndex].longitud = 111111111;
                customers[DataSource.Config.customersIndex].name = "Penina";
                customers[DataSource.Config.customersIndex].phone = "0548538699";
                DataSource.Config.customersIndex++;
                customers[DataSource.Config.customersIndex].id = 333333333;
                customers[DataSource.Config.customersIndex].latitude = 111111111;
                customers[DataSource.Config.customersIndex].longitud = 111111111;
                customers[DataSource.Config.customersIndex].name = "Yoni";
                customers[DataSource.Config.customersIndex].phone = "0544828416";
                DataSource.Config.customersIndex++;
                customers[DataSource.Config.customersIndex].id = 333333333;
                customers[DataSource.Config.customersIndex].latitude = 111111111;
                customers[DataSource.Config.customersIndex].longitud = 111111111;
                customers[DataSource.Config.customersIndex].name = "Chani";
                customers[DataSource.Config.customersIndex].phone = "0548459415";
                DataSource.Config.customersIndex++;
                customers[DataSource.Config.customersIndex].id = 444444444;
                customers[DataSource.Config.customersIndex].latitude = 111111111;
                customers[DataSource.Config.customersIndex].longitud = 111111111;
                customers[DataSource.Config.customersIndex].name = "Nomi";
                customers[DataSource.Config.customersIndex].phone = "0548459416";
                DataSource.Config.customersIndex++;
                customers[DataSource.Config.customersIndex].id = 555555555;
                customers[DataSource.Config.customersIndex].latitude = 111111111;
                customers[DataSource.Config.customersIndex].longitud = 111111111;
                customers[DataSource.Config.customersIndex].name = "Yosi";
                customers[DataSource.Config.customersIndex].phone = "0548735177";
                DataSource.Config.customersIndex++;
                customers[DataSource.Config.customersIndex].Id = 666666666;
                customers[DataSource.Config.customersIndex].latitude = 111111111;
                customers[DataSource.Config.customersIndex].longitud = 111111111;
                customers[DataSource.Config.customersIndex].name = "Avi";
                customers[DataSource.Config.customersIndex].phone = "0548735177";
                DataSource.Config.customersIndex++;
                customers[DataSource.Config.customersIndex].Id = 777777777;
                customers[DataSource.Config.customersIndex].latitude = 111111111;
                customers[DataSource.Config.customersIndex].longitud = 111111111;
                customers[DataSource.Config.customersIndex].name = "Talya";
                customers[DataSource.Config.customersIndex].phone = "0548735177";
                DataSource.Config.customersIndex++;
                customers[DataSource.Config.customersIndex].id = 888888888;
                customers[DataSource.Config.customersIndex].latitude = 111111111;
                customers[DataSource.Config.customersIndex].longitud = 111111111;
                customers[DataSource.Config.customersIndex].name = "Chaya";
                customers[DataSource.Config.customersIndex].phone = "0548735177";
                DataSource.Config.customersIndex++;
                customers[DataSource.Config.customersIndex].id = 999999999;
                customers[DataSource.Config.customersIndex].latitude = 111111111;
                customers[DataSource.Config.customersIndex].longitud = 111111111;
                customers[DataSource.Config.customersIndex].name = "Michal";
                customers[DataSource.Config.customersIndex].phone = "0548735177";
                DataSource.Config.customersIndex++;

                parcels[DataSource.Config.parcelsIndex].id = 121212121;
                parcels[DataSource.Config.parcelsIndex].paired = DateTime.Now;
                parcels[DataSource.Config.parcelsIndex].pickedUp = DateTime.Now;
                parcels[DataSource.Config.parcelsIndex].priority = (Priorities)1;
                parcels[DataSource.Config.parcelsIndex].senderId = 111111111;
                parcels[DataSource.Config.parcelsIndex].targetId = 111111111;
                Random r = new Random();
                r.nex
                parcels[DataSource.Config.parcelsIndex].weight = r;








            }

        }
    }
}

