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
        public void AddParcel(Parcel newParcel)
        {
            newParcel.Id = DataSource.Config.RunningParcelId++;
            newParcel.CreatParcel = DateTime.Now;
            newParcel.Delivered = null;
            newParcel.PickedUp = null;
            newParcel.Scheduled = null;
            DataSource.Parcels.Add(newParcel);
        }
        public Parcel GetParcel(int idParcel)
        {
            //search for the parcel that has the same id has the id that the user enterd
            int parcelIndex = DataSource.Parcels.FindIndex(item => item.Id == idParcel);
            if (parcelIndex == -1)
                throw new DoesNotExistException($"Parcel id: { idParcel } does not exists.");
            return DataSource.Parcels[parcelIndex];
        }
        public void DronToAParcel(int droneId, int parcelId)
        {
            //search for the drone that has the same id has the id that the user enterd
            int droneIndex = DataSource.Drones.FindIndex(item => item.Id == droneId);
            if (droneIndex == -1)
                throw new DoesNotExistException($"Drone id: {droneId} does not exist.");
            //search for the parcel that has the same id has the id that the user enterd
            int parcelIndex = DataSource.Parcels.FindIndex(item => item.Id == parcelId);
            if (parcelIndex == -1)
                throw new DoesNotExistException($"Parcel id: { parcelId } does not exists.");
            Parcel updateAParcel = DataSource.Parcels[parcelIndex];
            updateAParcel.DroneId = droneId;
            updateAParcel.Scheduled = DateTime.Now;
            DataSource.Parcels[parcelIndex] = updateAParcel;
        }
        public void PickUpParcel(int id)
        {
            //search for the parcel that has the same id has the id that the user enterd
            int parcelIndex = DataSource.Parcels.FindIndex(item => item.Id == id);
            if (parcelIndex == -1)
                throw new DoesNotExistException($"Parcel id: { id } does not exists.");
            Parcel updateAParcel = DataSource.Parcels[parcelIndex];
            updateAParcel.PickedUp = DateTime.Now;
            DataSource.Parcels[parcelIndex] = updateAParcel;
        }       
        public void ParcelToCustomer(int id)
        {
            //search for the parcel that has the same id has the id that the user enterd
            int parcelIndex = DataSource.Parcels.FindIndex(item => item.Id == id);
            if (parcelIndex == -1)
                throw new DoesNotExistException($"Parcel id: { id } does not exists.");
            Parcel updateAParcel = DataSource.Parcels[parcelIndex];
            updateAParcel.Delivered = DateTime.Now;
            updateAParcel.DroneId = 0;
            DataSource.Parcels[parcelIndex] = updateAParcel;
        }
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> predicate = null)
        {
            List<Parcel> Parcels = new();
            foreach (var itP in DataSource.Parcels)
                if (predicate == null || predicate(itP))
                    Parcels.Add(itP);
            return Parcels;
        }
    }
}
