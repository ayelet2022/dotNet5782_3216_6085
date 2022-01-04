using System.Xml.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;

using DO;

namespace Dal
{
    public class XMLTools
    {
        // static string dir = @"xml\";
        //static XMLTools()
        //{
        //    if (!Directory.Exists(dir))
        //        Directory.CreateDirectory(dir);
        //}

        #region SaveLoadWithXElement
        public static void SaveListToXMLElement(XElement rootElem, string filePath)
        {
            try
            {
                rootElem.Save(/*dir + */filePath);
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }

        public static XElement LoadListFromXMLElement(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    return XElement.Load(filePath);
                }
                else
                {
                    XElement rootElem = new XElement(/*dir + */filePath);
                    rootElem.Save(/*dir + */filePath);
                    return rootElem;
                }
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }
        #endregion

        #region SaveLoadWithXMLSerializer
        public static void SaveListToXMLSerializer<T>(List<T> list, string filePath)
        {
            try
            {
                FileStream file = new FileStream(filePath, FileMode.Create);
                XmlSerializer x = new XmlSerializer(list.GetType());
                x.Serialize(file, list);
                file.Close();
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }

        public static List<T> LoadListFromXMLSerializer<T>(string filePath)
        {
            try
            {
                if (File.Exists(/*dir + */filePath))
                {
                    List<T> list;
                    XmlSerializer x = new XmlSerializer(typeof(List<T>));
                    FileStream file = new FileStream(/*dir + */filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    list = (List<T>)x.Deserialize(file);
                    file.Close();
                    return list;
                }
                else
                {
                    return new List<T>();
                }
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }
        #endregion
    }
}
//XElement droneChargeRoot;
//string DCPath = @"DroneChargeXml.xml";
//public void SaveDroneCharge(IEnumerable<DroneCharge> droneCharges)
//{
//    droneChargeRoot = new XElement("droneCharges",
//    from item in droneCharges
//    select new XElement("droneCharge",
//    new XElement("DroneId", item.DroneId),
//    new XElement("StationId", item.StationId),
//    new XElement("StartCharging", item.StartCharging)
//    )
//    );
//    droneChargeRoot.Save(DCPath);
//}

//private void DroneChargeLoadData()
//{
//    try
//    {
//        droneChargeRoot = XElement.Load(DCPath);
//    }
//    catch
//    {
//        Console.WriteLine("File upload problem");
//    }
//}

//public IEnumerable<DroneCharge> GetDroneChargeList()
//{
//    DroneChargeLoadData();
//    IEnumerable<DroneCharge> droneCharges;
//    try
//    {
//        droneCharges = (from p in droneChargeRoot.Elements()
//                    select new DroneCharge()
//                    {
//                        DroneId = Convert.ToInt32(p.Element("DroneId").Value),
//                        StationId = Convert.ToInt32(p.Element("StationId").Value),
//                        StartCharging = Convert.ToDateTime(p.Element("StartCharging").Value),
//                    });
//    }
//    catch
//    {
//        droneCharges = null;
//    }
//    return droneCharges;
//}

//public DroneCharge GetDroneCharge(int id)
//{
//    DroneChargeLoadData();
//    DroneCharge droneCharge;
//    try
//    {
//        droneCharge = (from p in droneChargeRoot.Elements()
//                       where Convert.ToInt32(p.Element("DroneId").Value) == id
//                       select new DroneCharge()
//                        {
//                            DroneId = Convert.ToInt32(p.Element("DroneId").Value),
//                            StationId = Convert.ToInt32(p.Element("StationId").Value),
//                            StartCharging = Convert.ToDateTime(p.Element("StartCharging").Value),
//                        }).FirstOrDefault();
//    }
//    catch
//    {
//        droneCharge = default;
//    }
//    return droneCharge;
//}

//public void AddDroneCharge(DroneCharge droneCharge)
//{
//    XElement droneId = new XElement("DroneId", droneCharge.DroneId);
//    XElement stationId = new XElement("StationId", droneCharge.StationId);
//    XElement startCharging= new XElement("StartCharging", droneCharge.StartCharging);
//    droneChargeRoot.Add(new XElement("DroneCharge", droneId,stationId,startCharging));
//    droneChargeRoot.Save(DCPath);
//}

