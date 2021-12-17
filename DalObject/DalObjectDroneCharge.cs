using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;

namespace Dal
{
    internal sealed partial class DalObject : IDal
    {
        public void AddDroneCharge(DroneCharge droneCharge)
        {
            //chcks if the charger already exists
            if (DataSource.DroneCharges.Exists(item => item.DroneId == droneCharge.DroneId && item.StationId == droneCharge.StationId))
                throw new ExistsException($"The Drone:{droneCharge.DroneId} already charging.");
            DataSource.DroneCharges.Add(droneCharge);
        }
        public IEnumerable<DroneCharge> GetDroneCharges(Predicate<DroneCharge> predicate = null)
        {
            return DataSource.DroneCharges.Select(item => item);
        }
        public DroneCharge GetDroneCharge(int idDrone)
        {
            try
            {
                return DataSource.DroneCharges.First(x => x.DroneId == idDrone);

            }
            catch (Exception)
            {
                throw new DoesNotExistException($"Drone id: {idDrone} does not exist.");
            }
        }
    }
}
