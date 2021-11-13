using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
namespace BL
{
    class BLDrone
    {
        public void AddDrone(Drone drone)
        {

        }

        public Drone PrintDrone(int idDrone)
        {
            Drone drone = new();
            return drone;
        }

        /// <summary>
        /// copyes the values of all the drones in order to print them
        /// </summary>
        /// <returns>the new arrey that has the the drones</returns>
        public IEnumerable<Drone> GetDrones()
        {
            List<Drone> Drones = new List<Drone>();
            return Drones;
        }
    }
}
