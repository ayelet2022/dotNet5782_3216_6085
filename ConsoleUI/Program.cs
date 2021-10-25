using System;
using DAL.DalObject;
using DAL;
using DAL.IDAL.DO;
namespace ConsoleUI
{
    class Program 
    {
        static void Main(string[] args)
        {
            DalObject obj = new DalObject();
            string print="";
            print+=$"Enter 1 to add an object.\n";
            print+=$"Enter 2 to update an object.\n";
            print+=$"Enter 3 to print an object according to the id.\n";
            print+= $"Enter 4 to print the whole list of an object.\n";
            print+= $"Enter 5 to end the program.\n";
            Console.WriteLine(print);
            int Choice;
            int.TryParse(Console.ReadLine(), out Choice);
            while (Choice != 5)
            {
                switch (Choice)
                {
                    case 1:
                        string print1 = "";
                        print1+=$"Enter what would you like to add:\n";
                        print1+=$"Enter 1 to add a base station.\n";
                        print1+=$"Enter 2 to add a drone.\n";
                        print1+=$"Enter 3 to add a customer.\n";
                        print1+=$"Enter 4 to add a parcel.\n";
                        Console.WriteLine(print1);
                        int ChoiceAdd;
                        int.TryParse(Console.ReadLine(), out ChoiceAdd);
                        switch (ChoiceAdd)
                        {
                            case 1:
                                DalObject.AddBaseStation();
                                break;
                            case 2:
                                DalObject.AddDrone();
                                break;
                            case 3:
                                Customer newCustomer = new Customer();
                                Console.WriteLine("Enter Id (4 digits): ");
                                int idCustomer;
                                int.TryParse(Console.ReadLine(), out idCustomer);
                                newCustomer.Id = idCustomer;
                                Console.WriteLine("Enter Name: ");
                                string name = Console.ReadLine();
                                newCustomer.Name = name;
                                Console.WriteLine("Enter Phone: ");
                                string phone = Console.ReadLine();
                                newCustomer.Phone = phone;
                                Console.WriteLine("Enter Latitude: ");
                                double latitude;
                                double.TryParse(Console.ReadLine(), out latitude);
                                newCustomer.Latitude = latitude;
                                Console.WriteLine("Enter Longitude: ");
                                double longitude;
                                double.TryParse(Console.ReadLine(), out longitude);
                                newCustomer.Longitude = longitude;
                                DalObject.AddCustomer(newCustomer);
                                break;
                            case 4:
                                Parcel newParcel = new Parcel();
                                newParcel.Id = 0;
                                Console.WriteLine("Enter SenderId (4 digits): ");
                                int senderId;
                                int.TryParse(Console.ReadLine(), out senderId);
                                newParcel.SenderId = senderId;
                                Console.WriteLine("Enter TargetId (4 digits): ");
                                int targetId;
                                int.TryParse(Console.ReadLine(), out targetId);
                                newParcel.TargetId = targetId;
                                Console.WriteLine("Enter Weight (light=0/inbetween=1/heavy=2): ");
                                int weight;
                                int.TryParse(Console.ReadLine(), out weight);
                                newParcel.Weight = (WeightCategories)weight;
                                Console.WriteLine("Enter Priority (regular=0/fast=1/urgent=2): ");
                                int proiority;
                                int.TryParse(Console.ReadLine(), out proiority);
                                newParcel.Priority = (Priorities)proiority;
                                DalObject.AddParcel(newParcel);
                                break;
                        }
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
                                Console.WriteLine("Enter the id of the basestation you whant to charge the drone in (from the list): ");
                                BaseStation[] Stations = DAL.DalObject.DalObject.BaseStationWithAvailableCharges();
                                for (int i = 0; i < Stations.Length; i++)
                                    Console.WriteLine(Stations[i].ToString());
                                int IdOfBaseStation;
                                int.TryParse(Console.ReadLine(), out IdOfBaseStation);
                                DAL.DalObject.DalObject.DronToCharger(DronesId1, IdOfBaseStation);
                                break;
                            case 5:
                                Console.WriteLine("Enter the  drone Id (4 digits): ");
                                int DronesId2;
                                int.TryParse(Console.ReadLine(), out DronesId2);
                                DAL.DalObject.DalObject.FreeDroneFromBaseStation(DronesId2);
                                break;
                        }
                        break;
                    case 3:
                        String print3 = "";
                        print3 += $"Enter what would you like to do\n";
                        print3 += $"1 to present a station according to his id.\n";
                        print3 += $"2 to present a drone according to his id.\n";
                        print3 += $"3 to present a customer according to his id.\n";
                        print3 += $"4 to present a parcel according to his id.\n";
                        Console.WriteLine(print3);
                        int choice;
                        int.TryParse(Console.ReadLine(), out choice);
                        switch (choice)
                        {
                            case 1:
                                Console.WriteLine("Enter the base station id (4 digits)\n");
                                int id;
                                int.TryParse(Console.ReadLine(), out id);
                                Console.WriteLine(DalObject.PrintBaseStation(id).ToString());
                                break;
                            case 2:
                                Console.WriteLine("Enter the drone id (4 digits)\n");
                                int idDrone;
                                int.TryParse(Console.ReadLine(), out idDrone);
                                Console.WriteLine(DalObject.PrintDrone(idDrone).ToString());
                                break;
                            case 3:
                                Console.WriteLine("Enter the customer id (4 digits)\n");
                                int idCustomer;
                                int.TryParse(Console.ReadLine(), out idCustomer);
                                Console.WriteLine(DalObject.PrintCustomer(idCustomer).ToString());
                                break;
                            case 4:
                                Console.WriteLine("Enter the parcel id (4 digits)\n");
                                int idParcel;
                                int.TryParse(Console.ReadLine(), out idParcel);
                                Console.WriteLine(DalObject.PrintParcel(idParcel).ToString());
                                break;
                        }
                        break;
                    case 4:
                        Console.WriteLine("What list would you like to print?");
                        string PrintChoice = Console.ReadLine();
                        switch (PrintChoice)
                        {
                            case "BaseStation":
                                BaseStation[] ActiveStations = DAL.DalObject.DalObject.PrintBaseStations();
                                for (int i = 0; i < ActiveStations.Length; i++)
                                    Console.WriteLine(ActiveStations[i].ToString());
                                break;
                            case "Drone":
                                Drone[] ActiveDrones = DAL.DalObject.DalObject.PrintDrones();
                                for (int i = 0; i < ActiveDrones.Length; i++)
                                    Console.WriteLine(ActiveDrones[i].ToString());
                                break;
                            case "Customer":
                                Customer[] ActiveCustomers = DAL.DalObject.DalObject.PrintCustomers();
                                for (int i = 0; i < ActiveCustomers.Length; i++)
                                    Console.WriteLine(ActiveCustomers[i].ToString());
                                break;
                            case "Parcel":
                                Parcel[] ActiveParcels = DAL.DalObject.DalObject.PrintPercels();
                                for (int i = 0; i < ActiveParcels.Length; i++)
                                    Console.WriteLine(ActiveParcels[i].ToString());
                                break;
                            case "Parcel_that_weren't_paired":
                                Parcel[] Parcels = DAL.DalObject.DalObject.ParcelThatWerenNotPaired();
                                for (int i = 0; i < Parcels.Length; i++)
                                    Console.WriteLine(Parcels[i].ToString());
                                break;
                            case "BaseStation_with_available_charges":
                                BaseStation[] Stations = DAL.DalObject.DalObject.BaseStationWithAvailableCharges();
                                for (int i = 0; i < Stations.Length; i++)
                                    Console.WriteLine(Stations[i].ToString());
                                break;
                        }
                        break;
                }
                Console.WriteLine("Enter 1 to add an object, ");
                Console.WriteLine("Enter 2 to update an object, ");
                Console.WriteLine("Enter 3 to print an object according to the id, ");
                Console.WriteLine("Enter 4 to print the whole list of an object, ");
                Console.WriteLine("Enter 5 to end the program, ");
                int.TryParse(Console.ReadLine(), out Choice);
            }
        }
    }
}
