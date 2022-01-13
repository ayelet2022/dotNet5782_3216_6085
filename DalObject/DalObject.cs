using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DO;
using DalApi;

namespace Dal
{
    internal sealed partial class DalObject : IDal
    {
        internal static DalObject Instance { get { return instance.Value; } }
        private static readonly Lazy<DalObject> instance = new Lazy<DalObject>(() => new DalObject());
        static DalObject() { }
        private DalObject() => DataSource.Initialize();

        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] AskForBattery()
        {
            double[] arr = { DataSource.Config.Available, 
                             DataSource.Config.Light, 
                             DataSource.Config.MediumWeight, 
                             DataSource.Config.Heavy, 
                             DataSource.Config.ChargingRate };
            return arr;
        }
    }
}

     



