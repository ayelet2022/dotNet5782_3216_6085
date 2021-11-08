using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using DAL;
namespace DalObject
{
    public partial class DalObject
    {
        /// <summary>
        /// adds a new base station to the arrey
        /// </summary>
        public void AddBaseStation(BaseStation addBaseStation)
        {
            DataSource.Stations.Add(addBaseStation);
        }

        /// <summary>
        ///  search for the base station in the arrey thet has the same id that the user enterd and returnes it
        /// </summary>
        /// <param name="idBaseStation"><the id(that was enterd by the user in main) of the basestation that the user wants to print/param>
        /// <returns>resturn the base station that needs to be printed</returns>
        public BaseStation PrintBaseStation(int idBaseStation)
        {
            int baseStationIndex = 0;
            while (DataSource.Stations[baseStationIndex].Id != idBaseStation)//search for the base station that has the same id has the id that the user enterd
                baseStationIndex++;
            return DataSource.Stations[baseStationIndex];

        }

        /// <summary>
        /// copyes the values of al the base stations in order to print them
        /// </summary>
        /// <returns>the new arrey that has the the base stations</returns>
        public IEnumerable<BaseStation> PrintBaseStations()
        {
            return DataSource.Stations;
        }

        /// <summary>
        /// search for tha base station that has available charges and copys them to a new arrey
        /// </summary>
        /// <returns>the new arrey that has all the base station that has free charges</returns>
        public IEnumerable<BaseStation> BaseStationWithAvailableCharges()
        {
            List<BaseStation> Stations = new();
            foreach (var itBS in DataSource.Stations)
            {
                if (itBS.EmptyCharges > 0)
                    Stations.Add(itBS);
            }
            return Stations;
        }
        public int searchBaseStation(int id)
        {
            int index = 0;
            foreach (var itBS in DataSource.Stations )
            {
                if(itBS.Id==id)
                    return index;
                index++;
            }
            return -1;
        }

    }
}
