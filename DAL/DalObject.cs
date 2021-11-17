using System;
using System.Collections.Generic;
using IDAL.DO;
using IDAL;
namespace DalObject
{
     public partial class DalObject : IDAL.IDal
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

        public IEnumerable<Parcel> GetDroneCharge()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Drone> PrintDrones()
        {
            throw new NotImplementedException();
        }
    }
}

     



