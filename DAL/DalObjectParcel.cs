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
        /// <summary>
        /// adds a new parcel to the arrey
        /// </summary>
        /// <param name="newParcel">the new parce that the user entered in main and needs to be added to the arrey</param>
        public void AddParcel(Parcel newParcel)
        {
            newParcel.Id = DataSource.Config.RunningParcelId++;
            newParcel.CreatParcel = DateTime.MinValue;
            newParcel.Delivered = DateTime.MinValue;
            newParcel.PickedUp = DateTime.MinValue;
            newParcel.Scheduled = DateTime.MinValue;
            DataSource.Parcels.Add(newParcel);
        }

        /// <summary>
        /// search for the parcel in the arrey thet has the same id as the user enterd and retirnes it
        /// </summary>
        /// <param name="idParcel">the id(that was enterd by the user in main) of the parcel that the user wants to print</param> 
        /// <returns>the parcel that needs to be printed</returns>
        public Parcel GetParcel(int idParcel)
        {
            if (DataSource.Parcels.Exists(item => item.Id != idParcel))
                    throw new DoesNotExistException("Parcel does not exists.");
                int parcelIndex = 0;
            while (DataSource.Parcels[parcelIndex].Id != idParcel)//search for the parcel that has the same id has the id that the user enterd
                parcelIndex++;
            return (DataSource.Parcels[parcelIndex]);

        }

        /// <summary>
        /// search for the drone and the parcel that has the same id as what the user enterd 
        /// and changes the filed of the drone id inn the parcel detailes
        /// </summary>
        /// <param name="droneId">the drones id that the user enterd</param>
        /// <param name="parcelId">the parcel id that the user enterd</param>
        public void DronToAParcel(int droneId, int parcelId)
        {
            if (DataSource.Drones.Exists(item => item.Id != droneId))
                throw new DoesNotExistException("Drone does not exists.");
            if (DataSource.Parcels.Exists(item => item.Id != parcelId))
                throw new DoesNotExistException("Parcel does not exists.");
            int pareclIndex = 0;
            while (DataSource.Parcels[pareclIndex].Id != parcelId)//search for the parcel that has the same id has the id that the user enterd
                pareclIndex++;
            Parcel updateAParcel = DataSource.Parcels[pareclIndex];
            updateAParcel.DroneId = droneId;
            updateAParcel.Scheduled = DateTime.Now;
            updateAParcel.PickedUp = DateTime.Now;
            DataSource.Parcels[pareclIndex] = updateAParcel;
        }

        /// <summary>
        /// updates the time that the parcel was picked up by the dron
        /// </summary>
        /// <param name="newId">the id pf the parcel that was enterd by the user</param>
        public void PickUpParcel(int newId)
        {
            if (DataSource.Parcels.Exists(item => item.Id != newId))
                throw new DoesNotExistException("Parcel does not exists.");
            int parcelsIndex = 0;
            while (DataSource.Parcels[parcelsIndex].Id != newId)//search for the parcel that has the same id has the id that the user enterd
                parcelsIndex++;
            Parcel updateAParcel = DataSource.Parcels[parcelsIndex];
            updateAParcel.PickedUp = DateTime.Now;
            DataSource.Parcels[parcelsIndex] = updateAParcel;
        }

        /// <summary>
        /// updates the time that the parcel was deleverd to the customer
        /// </summary>
        /// <param name="newId">the id pf the parcel that was enterd by the user</param>
        public void ParcelToCustomer(int newId)
        {
            if (DataSource.Parcels.Exists(item => item.Id != newId))
                throw new DoesNotExistException("Parcel does not exists.");
            int parcelsIndex = 0;
            while (DataSource.Parcels[parcelsIndex].Id != newId)//search for the parcel that has the same id has the id that the user enterd
                parcelsIndex++;
            Parcel updateAParcel = DataSource.Parcels[parcelsIndex];
            updateAParcel.Delivered = DateTime.Now;
            updateAParcel.DroneId = 0;
            DataSource.Parcels[parcelsIndex] = updateAParcel;
        }

        /// <summary>
        /// copyes the values of all the parcel in order to print them
        /// </summary>
        /// <returns>the new arrey that has the the parceles</returns>
        public IEnumerable<Parcel> GetParcels()
        {
            List<Parcel> Parcels = new();
            foreach (var itBS in DataSource.Parcels)
            {
                    Parcels.Add(itBS);
            }
            return Parcels;
        }

        /// <summary>
        /// search for all the drones that are available and copy them to a new arrey  
        /// </summary>
        /// <returns>the new arrey that has all the drones that are availeble</returns>
        public IEnumerable<Parcel> GetParcelThatWerenNotPaired()
        {
            List<Parcel> Parcels = new();
            foreach (var itBS in DataSource.Parcels)
            {
                if (itBS.DroneId == 0)
                    Parcels.Add(itBS);
            }
            return Parcels;
        }
    }
}
