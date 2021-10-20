using System;
using DAL;
using IDAL.DO;
Random rand = new Random(DateTime.Now.Millisecond);

namespace DalObject
{
    class DataSource
    {
        static Drone[] drones = new Drone[10];
        static BaseStation[] stations = new BaseStation[5];
        static Customer[] customers = new Customer[100];
        static Parcel[] parcels = new Parcel[1000];
        internal class Config
        {
            internal static int dronesIndex = 0;
            internal static int stationsIndex = 0;
            internal static int customersIndex = 0;
            internal static int parcelsIndex = 0;
            internal int i = 0;
            public int DronesIndex
            {
                get => dronesIndex;
                set { dronesIndex = value; }
            }
            static void Initialize()
            {
                drones[0].id = rand(0,10);
                drones[0].battery = 100;
                drones[0].id = 1111;
                drones[0].maxWeight = (WeightCategories)0;
                drones[0].model = "aaa";
                drones[0].status = (DroneStatuses)0;

                drones[dronesIndex].battery = 90;
                drones[1].id = 2222;
                drones[1].maxWeight = (WeightCategories)1;
                drones[1].model = "bbb";
                drones[1].status = (DroneStatuses)1;

                drones[dronesIndex].battery = 80;
                drones[2].id = 3333;
                drones[2].maxWeight = (WeightCategories)2;
                drones[2].model = "ccc";
                drones[2].status = (DroneStatuses)2;

                drones[dronesIndex].battery = 70;
                drones[3].id = 4444;
                drones[3].maxWeight = (WeightCategories)0;
                drones[1].model = "ddd";
                drones[1].status = (DroneStatuses)1;

                drones[dronesIndex].battery = 60;
                drones[1].id = 4444;
                drones[1].maxWeight = (WeightCategories)1;
                drones[1].model = "yyy";
                drones[1].status = (DroneStatuses)1;

                stations[0].emptyCharges = 4;
                stations[0].id = 264;
                stations[0].latitude = 26.8;
                stations[0].longitud = 50;
                stations[0].name = "jerusalem";

                stations[0].emptyCharges = 4;
                stations[0].id = 264;
                Random r = new Random();
                stations[0].latitude = r;
                stations[0].longitud = 50;
                stations[0].name = "jerusalem";

                drones[1].model = "eee";
                drones[1].status = (DroneStatuses)2;

                customers[0].id =
            }
       
        }
        

    }
}

