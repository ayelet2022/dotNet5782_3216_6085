using System;
using DAL;
 namespace IDAL
 {
     namespace DO
     {
        public enum WeightCategories { light, inbetween, heavy };
        public enum DroneStatuses { available,inFix, delivery};
       
     }
  

 }
namespace DalObject
{
    class DataSource
    {
        internal static Drone[] drones = new Drone[10];

    }
}

