using System;
using System.Collections.Generic;
using DO;
using DalApi;
using StructureMap.Pipeline;

namespace DalObject
{
    public partial class DalObject : IDal
    {
        static IDal inst = new DalObject();
        DalObject()
        {
            DataSource.Initialize();
        }
        public static Instance {get => inst; }
        public double[] AskForBattery()
        {
            double[] arr = { DataSource.Config.Available, DataSource.Config.Light, DataSource.Config.MediumWeight, DataSource.Config.Heavy, DataSource.Config.ChargingRate };
            return arr;
        }
    }
}

     



