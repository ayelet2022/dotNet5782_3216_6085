using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using DO;
using DalApi;

namespace Dal
{
    internal sealed partial class DalObject : IDal
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddParcel(Parcel newParcel)
        {
            newParcel.Id = DataSource.Config.RunningParcelId++;
            newParcel.CreatParcel = DateTime.Now;
            newParcel.Delivered = null;
            newParcel.PickedUp = null;
            newParcel.Scheduled = null;
            newParcel.IsActive = true;
            DataSource.Parcels.Add(newParcel);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int idParcel)
        {
            try
            {
                //search for the parcel that has the same id has the id that the user enterd
                Parcel parcel = DataSource.Parcels.First(item => item.Id == idParcel);
                if (parcel.IsActive)
                    return parcel;
                else
                    throw new ItemIsDeletedException();
            }
            catch (InvalidOperationException ex)
            {
                throw new DoesNotExistException($"Parcel id: { idParcel } does not exists.");
            }
            catch (ItemIsDeletedException ex)
            {
                throw new DoesNotExistException($"Parcel id: { idParcel } does not exists.");
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DronToAParcel(int droneId, int parcelId)
        {
            //search for the drone that has the same id has the id that the user enterd
            int droneIndex = DataSource.Drones.FindIndex(item => item.Id == droneId);
            if (droneIndex == -1)
                throw new DoesNotExistException($"Drone id: {droneId} does not exist.");
            //search for the parcel that has the same id has the id that the user enterd
            int parcelIndex = DataSource.Parcels.FindIndex(item => item.Id == parcelId && item.IsActive);
            if (parcelIndex == -1)
                throw new DoesNotExistException($"Parcel id: { parcelId } does not exists.");
            Parcel updateAParcel = DataSource.Parcels[parcelIndex];
            updateAParcel.DroneId = droneId;
            updateAParcel.Scheduled = DateTime.Now;
            DataSource.Parcels[parcelIndex] = updateAParcel;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PickUpParcel(int id)
        {
            //search for the parcel that has the same id has the id that the user enterd
            int parcelIndex = DataSource.Parcels.FindIndex(item => item.Id == id && item.IsActive);
            if (parcelIndex == -1)
                throw new DoesNotExistException($"Parcel id: { id } does not exists.");
            Parcel updateAParcel = DataSource.Parcels[parcelIndex];
            updateAParcel.PickedUp = DateTime.Now;
            DataSource.Parcels[parcelIndex] = updateAParcel;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ParcelToCustomer(int id)
        {
            //search for the parcel that has the same id has the id that the user enterd
            int parcelIndex = DataSource.Parcels.FindIndex(item => item.Id == id && item.IsActive);
            if (parcelIndex == -1)
                throw new DoesNotExistException($"Parcel id: { id } does not exists.");
            Parcel updateAParcel = DataSource.Parcels[parcelIndex];
            updateAParcel.Delivered = DateTime.Now;
            updateAParcel.DroneId = 0;
            DataSource.Parcels[parcelIndex] = updateAParcel;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> predicate = null)
        {
            return from item in DataSource.Parcels
                   where predicate == null ? true : predicate(item)
                   where item.IsActive == true
                   select item;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteParcel(int id)
        {
            try
            {
                Parcel parcel = GetParcel(id);
                if (parcel.Scheduled == null)
                    DataSource.Parcels.Remove(parcel);
                else
                {
                    parcel.IsActive = false;
                    int parcelIndex = DataSource.Parcels.FindIndex(item => item.Id == id);
                    DataSource.Parcels[parcelIndex] = parcel;
                }
            }
            catch (DoesNotExistException ex)
            {
                throw new ItemIsDeletedException($"Parcel: { id } is already deleted.");
            }
        }
    }
}
