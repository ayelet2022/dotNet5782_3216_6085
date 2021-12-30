using System.Xml.Linq;
using DO;
class DalXml
{
    XElement droneChargeRoot;
    string DCPath = @"DroneChargeXml.xml";
    public void SaveDroneCharge(IEnumerable<DroneCharge> droneCharges)
    {
        droneChargeRoot = new XElement("droneCharges",
        from item in droneCharges
        select new XElement("droneCharge",
        new XElement("DroneId", item.DroneId),
        new XElement("StationId", item.StationId),
        new XElement("StartCharging", item.StartCharging)
        )
        );
        droneChargeRoot.Save(DCPath);
    }

    private void DroneChargeLoadData()
    {
        try
        {
            droneChargeRoot = XElement.Load(DCPath);
        }
        catch
        {
            Console.WriteLine("File upload problem");
        }
    }

    public IEnumerable<DroneCharge> GetDroneChargeList()
    {
        DroneChargeLoadData();
        IEnumerable<DroneCharge> droneCharges;
        try
        {
            droneCharges = (from p in droneChargeRoot.Elements()
                        select new DroneCharge()
                        {
                            DroneId = Convert.ToInt32(p.Element("DroneId").Value),
                            StationId = Convert.ToInt32(p.Element("StationId").Value),
                            StartCharging = Convert.ToDateTime(p.Element("StartCharging").Value),
                        });
        }
        catch
        {
            droneCharges = null;
        }
        return droneCharges;
    }

    public DroneCharge GetDroneCharge(int id)
    {
        DroneChargeLoadData();
        DroneCharge droneCharge;
        try
        {
            droneCharge = (from p in droneChargeRoot.Elements()
                           where Convert.ToInt32(p.Element("DroneId").Value) == id
                           select new DroneCharge()
                            {
                                DroneId = Convert.ToInt32(p.Element("DroneId").Value),
                                StationId = Convert.ToInt32(p.Element("StationId").Value),
                                StartCharging = Convert.ToDateTime(p.Element("StartCharging").Value),
                            }).FirstOrDefault();
        }
        catch
        {
            droneCharge = default;
        }
        return droneCharge;
    }

    public void AddDroneCharge(DroneCharge droneCharge)
    {
        XElement droneId = new XElement("DroneId", droneCharge.DroneId);
        XElement stationId = new XElement("StationId", droneCharge.StationId);
        XElement startCharging= new XElement("StartCharging", droneCharge.StartCharging);
        droneChargeRoot.Add(new XElement("DroneCharge", droneId,stationId,startCharging));
        droneChargeRoot.Save(DCPath);
    }

}
