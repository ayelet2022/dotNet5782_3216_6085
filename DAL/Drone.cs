using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DAL
{
  
    public struct Drone
    {
       public int id { get; set; }
       public string model { get; set; }
       public WeightCategories maxWeight { get; set; }
       public double battery { get; set; }
       public DroneStatuses status { get; set; }
    }
}
