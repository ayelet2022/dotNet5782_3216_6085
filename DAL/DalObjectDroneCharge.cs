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
        public void AddDroneCharge(DroneCharge droneCharge)
        {
            //chcks if the charger already exists
            if (DataSource.DroneCharges.Exists(item => item.DroneId == droneCharge.DroneId && item.StationId== droneCharge.StationId))
                throw new ExistsException($"The Drone:{droneCharge.DroneId} already charging.");
            DataSource.DroneCharges.Add(droneCharge);
        }
        public IEnumerable<DroneCharge> GetDroneCharge()
        {
            List<DroneCharge> DronesCharge = new();
            //to copy all the charger from the list
            foreach (var itD in DataSource.DroneCharges)
                DronesCharge.Add(itD);
            return DronesCharge;
        }
    }
}
