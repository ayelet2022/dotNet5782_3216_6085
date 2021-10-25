using System;
using DAL;
namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter 1 to add an object, ");
            Console.WriteLine("Enter 2 to update an object, ");
            Console.WriteLine("Enter 3 to print an object according to the id, ");
            Console.WriteLine("Enter 4 to print the whole list of an object, ");
            Console.WriteLine("Enter 5 to end the program, ");
            int Choice;
            int.TryParse(Console.ReadLine(), out Choice);
            switch (Choice)
            {
                case 1:
                    break;
                case 2:
                    String result = " ";
                    result += "What would you want to update? \n";
                    result += "Enter 1 to conect a dron to a parcel\n";
                    result += "Enter 2 to pickup a percel from the dron\n";
                    result += "Enter 3 to deliver a percel to a customer\n";
                    result += "Enter 4 to send a dron to a charge station\n";
                    result += "Enter 5 to free a dron from a charge station\n";
                    Console.WriteLine(result);
                    int.TryParse(Console.ReadLine(), out Choice);
                    switch (Choice)
                    {
                        case 1:
                            Console.WriteLine("Enter the  percel Id (4 digits): ");
                            int PercelId1;
                            int.TryParse(Console.ReadLine(), out PercelId1);
                            DAL.DalObject.DalObject.DronToAParcel(PercelId1);
                            break;
                        case 2:
                            Console.WriteLine("Enter the  percel Id (4 digits): ");
                            int PercelId2;
                            int.TryParse(Console.ReadLine(), out PercelId2);
                            DAL.DalObject.DalObject.PickUpParcel(PercelId2);
                            break;
                        case 3:
                            Console.WriteLine("Enter the  percel Id (4 digits): ");
                            int PercelId3;
                            int.TryParse(Console.ReadLine(), out PercelId3);
                            DAL.DalObject.DalObject.ParcelToCustomer(PercelId3);
                            break;
                        case 4:
                            Console.WriteLine("Enter the  drone Id (4 digits): ");
                            int DronesId1;
                            int.TryParse(Console.ReadLine(), out DronesId1);
                            Console.WriteLine("Enter the name of the basestation you whant to charge the drone in: ");
                            string NameOfBaseStation = Console.ReadLine();
                            DAL.DalObject.DalObject.DronToCharger(DronesId1, NameOfBaseStation);//לבדוק בפרוגרם
                            break;
                        case 5:
                            Console.WriteLine("Enter the  drone Id (4 digits): ");
                            int DronesId2;
                            int.TryParse(Console.ReadLine(), out DronesId2);
                            DAL.DalObject.DalObject.FreeDroneFromBaseStation(DronesId2);//לבדוק בפרוגרם
                            break;
                    }
                    break;
                case 3:
                    break;
                case 4:
                    Console.WriteLine("What list would you like to print?");
                    string PrintChoice = Console.ReadLine();
                    switch (PrintChoice)
                    {
                        case "BaseStation":
                            BaseStation[] ActiveStations = DAL.DalObject.DalObject.PrintBaseStations();
                            for (int i = 0; i < ActiveStations.Length; i++)
                                ActiveStations.ToString();
                            break;
                        case "Drone":
                            Drone[] ActiveDrones = DAL.DalObject.DalObject.PrintDrones();
                            for (int i = 0; i < ActiveDrones.Length; i++)
                                ActiveDrones[i].ToString();
                            break;
                        case "Customer":
                            Customer[] ActiveCustomers = DAL.DalObject.DalObject.PrintCustomers();
                            for (int i = 0; i < ActiveCustomers.Length; i++)
                                ActiveCustomers[i].ToString();
                            break;
                        case "Parcel":
                            Parcel[] ActiveParcels = DAL.DalObject.DalObject.PrintPercels();
                            for (int i = 0; i < ActiveParcels.Length; i++)
                                ActiveParcels[i].ToString();
                            break;
                        case "Parcel_that_weren't_paired":
                            Parcel[] Parcels = DAL.DalObject.DalObject.ParcelThatWerenNotPaired();
                            for (int i = 0; i < Parcels.Length; i++)
                                Parcels[i].ToString();
                            break;
                        case "BaseStation_with_available_charges":
                            BaseStation[] Stations = DAL.DalObject.DalObject.BaseStationWithAvailableCharges();
                            for (int i = 0; i < Stations.Length; i++)
                                Stations[i].ToString();
                            break;
                    }
                    break;
            }
        }
    }
}
