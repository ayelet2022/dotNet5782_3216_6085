using System;
using System.Collections.Generic;
using DO;
using DalApi;
using StructureMap.Pipeline;

namespace DalObject
{
    public partial class DalObject : DalApi.IDal
    {
        internal static BL Instance= new BL();
        DalObject()
        {
            DataSource.Initialize();
        }
        public double[] AskForBattery()
        {
            double[] arr = { DataSource.Config.Available, DataSource.Config.Light, DataSource.Config.MediumWeight, DataSource.Config.Heavy, DataSource.Config.ChargingRate };
            return arr;
        }
    }
}

     



