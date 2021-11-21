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
        public void DeleteDroneFromeCharger(int droneId)
        {
            int i = 0;
            foreach (var item in DataSource.DroneCharges)
            {
                if (item.DroneId == droneId)
                    break;
                i++;
            }
            DroneCharge dron=new();
            dron.DroneId = 0;
            dron.StationId = 0;
            DataSource.DroneCharges[i] = dron;
        }
    }
}
