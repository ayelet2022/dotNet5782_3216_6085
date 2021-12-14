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
            List<DroneCharge> DronesCharge = new();
            //to copy all the charger from the list
            foreach (var itD in DataSource.DroneCharges)
                if (predicate == null || predicate(itD))
                    DronesCharge.Add(itD);
            return DronesCharge;
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
