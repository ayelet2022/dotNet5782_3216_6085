using System;
using DAL;
 namespace IDAL
 {
     namespace DO
     {
        public enum WeightCategories { light, inbetween, heavy };
        public enum DroneStatuses { available,inFix, delivery};
        public enum Priorities { regular, fast, urgent };

    }
  

 }
namespace DalObject
{
    class DataSource
    {
        static Drone[] drones = new Drone[10];
        static BaseStation[] stations = new BaseStation[5];
        static Customer[] customers = new Customer[100];
        static Parcel[] parcels = new Parcel[1000];
        class Config
        {
            static int dronesIndex = 0;
            static int stationsIndex = 0;
            static int customersIndex = 0;
            static int parcelsIndex = 0;
            int i = 0;
        }
        static void Initialize()
        {
            drones[dronesIndex].battery = 100;
            drones[0].battery = 100;

            stations[0].emptyCharges = 4;
        }

    }
}

