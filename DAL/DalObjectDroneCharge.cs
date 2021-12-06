using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
namespace DalObject
{
    public partial class DalObject
    {
        //public void AddDroneCharge(DroneCharge droneCharge)
        //{
        //    //chcks if the charger already exists
        //    if (DataSource.DroneCharges.Exists(item => item.DroneId == droneCharge.DroneId && item.StationId == droneCharge.StationId))
        //        throw new ExistsException($"The Drone:{droneCharge.DroneId} already charging.");
        //    DataSource.DroneCharges.Add(droneCharge);
        //}
        public IEnumerable<DroneCharge> GetDroneCharges(Predicate<DroneCharge> predicate = null)
        {
            List<DroneCharge> DronesCharge = new();
            //to copy all the charger from the list
            foreach (var itD in DataSource.DroneCharges)
                if (predicate == null || predicate(itD))
                    DronesCharge.Add(itD);
            return DronesCharge;
        }

        public DroneCharge GetDroneCharge(Predicate<DroneCharge> predicate = null)
        {
            DroneCharge DronesCharge = new();
            //to copy all the charger from the list
            foreach (var itD in DataSource.DroneCharges)
                if (predicate == null || predicate(itD))
                    DronesCharge=itD;
            return DronesCharge;
        }
    }
}
