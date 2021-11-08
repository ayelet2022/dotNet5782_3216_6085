using System;
using System.Collections.Generic;
using DAL;
using IDAL.DO;
using IDAL;
namespace DalObject
{
     public partial class DalObject:IDal
    {
        public DalObject()
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

     



