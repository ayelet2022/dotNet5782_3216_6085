using System;
using System.Collections.Generic;
using DO;
using DalApi;

namespace Dal
{
    internal sealed partial class DalObject : IDal
    {
        internal static readonly DalObject Instance = new DalObject();
        private DalObject()
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

     



