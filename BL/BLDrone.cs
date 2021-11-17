using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
namespace BL
{
    public partial class BL
    {
        public void AddDrone(Drone drone,int idFirstStation)
        {
            try
            {
                if (drone.Id < 100000 || drone.Id > 999999)
                    throw new InvalidInputException("The drones id is incorrect");
                if ((int)drone.MaxWeight < 0 || (int)drone.MaxWeight > 3)
                    throw new InvalidInputException("The drones weight is incorrect");
                if (drone.Model.Length != 6)
                    throw new InvalidInputException("The drones model is incorrect");
                if (idFirstStation < 1000 || idFirstStation > 9999)
                    throw new InvalidInputException("The id of the baseStation incorrect");

            }
            catch (Exception)
            {

                throw;
            }
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
