using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace BL
{
    class BLBaseStation
    {
        public void AddBaseStation(BaseStation baseStation)
        {
            try
            {
                if (baseStation.Id > 9999 || baseStation.Id < 1000)
                    throw new InvalidInputException("The baseStations id is incorrect");
                if(baseStation.Name == "\n")
                    throw new InvalidInputException("The baseStations name in incorrect");
            }
            catch(IDAL.DO.ExistsException ex)
            {
                throw new FailedToAddException(ex.ToString(), ex);
            }
        }

        public BaseStation PrintBaseStation(int idBaseStation)
        {
            BaseStation station = new();
            return station;
        }

        /// <summary>
        /// copyes the values of al the base stations in order to print them
        /// </summary>
        /// <returns>the new arrey that has the the base stations</returns>
        public IEnumerable<BaseStation> PrintBaseStations()
        {
            List<BaseStation> Stations = new List<BaseStation>();
            return (IEnumerable<BaseStation>)Stations;
        }

        /// <summary>
        /// search for tha base station that has available charges and copys them to a new arrey
        /// </summary>
        /// <returns>the new arrey that has all the base station that has free charges</returns>
        public IEnumerable<BaseStation> BaseStationWithAvailableCharges()
        {
            List<BaseStation> Stations = new();
            return Stations;
        }
    }
}
