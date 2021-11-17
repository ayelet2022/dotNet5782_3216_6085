using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
namespace DalObject
{
    partial class DalObjectDroneCharge
    {
        public void AddDroneCharge(DroneCharge droneCharge)
        {
            if (DataSource.DroneCharges.Exists(item => item.DroneId == droneCharge.DroneId&&item.StationId== droneCharge.StationId))
                throw new ExistsException($"The Drone:{droneCharge.DroneId} already charging.");
            DataSource.DroneCharges.Add(droneCharge);
        }
        public IEnumerable<DroneCharge> GetDroneCharge()
        {
            List<DroneCharge> DronesCharge = new();
            foreach (var itD in DataSource.DroneCharges)
            {
                DronesCharge.Add(itD);
            }
            return DronesCharge;
        }
    }
}
