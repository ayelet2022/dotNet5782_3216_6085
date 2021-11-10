using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
namespace DalObject
{
    partial class DalObjectDroneCharge
    {
        public IEnumerable<DroneCharge> GetDroneCharge()
        {
            List<DroneCharge> DronesCharge = new();
            foreach (var itD in DataSource.DroneCharges)
            {
                DronesCharge.Add(itD);
            }
            return DronesCharge;
        }
    }
}
