using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
namespace BL
{
    class BLParcel
    {
        public void AddParcel(Parcel parcel)
        {

        }

        public Parcel PrintParcel(int idParcel)
        {
            Parcel parcel = new();
            return parcel;
        }

        /// <summary>
        /// copyes the values of all the parcel in order to print them
        /// </summary>
        /// <returns>the new arrey that has the the parceles</returns>
        public IEnumerable<Parcel> GetParcels()
        {
            List<Parcel> Parcels = new();
            return Parcels;
        }

        /// <summary>
        /// search for all the drones that are available and copy them to a new arrey  
        /// </summary>
        /// <returns>the new arrey that has all the drones that are availeble</returns>
        public IEnumerable<Parcel> ParcelThatWerenNotPaired()
        {
            List<Parcel> Parcels = new();
            return Parcels;
        }
    }
}
