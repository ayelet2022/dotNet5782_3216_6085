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
        public void AddParcel(Parcel parcel)
        {
            if (parcel.Id < 100000000 || parcel.Id > 999999999)
                throw new InvalidInputException("The parcel id is incorrect");
            if (parcel.Sender.Id < 100000000 || parcel.Sender.Id > 999999999)
                throw new InvalidInputException("The parcel id is incorrect");
            parcel.CreatParcel = DateTime.Now;
            parcel.Delivered = DateTime.MinValue;
            parcel.PickedUp = DateTime.MinValue;
            parcel.Scheduled = DateTime.MinValue;
            parcel.ParecelDrone = null;
            IDAL.DO.Parcel newParcel = new();
            newParcel.CopyPropertiesTo(newParcel);
            dal.AddParcel(newParcel);
        }

        public Parcel GetParcel(int idParcel)
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
