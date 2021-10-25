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
            string Choice= Console.ReadLine();
            switch (Choice)
            {
                case "add":
                    Console.WriteLine("Enter what would you like to add:");
                    Console.WriteLine("Enter 1 to add a base station");
                    Console.WriteLine("Enter 2 to add a drone");
                    Console.WriteLine("Enter 3 to add a customer");
                    Console.WriteLine("Enter 4 to add a parcel");
                    int ChoiceAdd;
                    int.TryParse(Console.ReadLine(), out ChoiceAdd);
                    switch(ChoiceAdd)
                    {

                        case 1:
                           DalObject.AddBaseStation();
                            break;
                        case 2:
                            DalObject.AddDrone();
                            break;
                        case 3:
                            Customer newCustomer=new Customer();
                            Console.WriteLine("Enter Id: ");
                            int idCustomer;
                            int.TryParse(Console.ReadLine(), out idCustomer);
                            newCustomer.Id = idCustomer;
                            Console.WriteLine("Enter Name: ");
                            string name = Console.ReadLine();
                            newCustomer.Name = name;
                            Console.WriteLine("Enter Phone: ");
                            string phone=Console.ReadLine();
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
                            Parcel newParcel=new Parcel();
                            int idParcel;
                            Console.WriteLine("Enter Parcel Id: ");
                            int.TryParse(Console.ReadLine(), out idParcel);
                            newParcel.Id = idParcel;
                            Console.WriteLine("Enter SenderId: ");
                            int senderId;
                            int.TryParse(Console.ReadLine(), out senderId);
                            newParcel.SenderId = senderId;
                            Console.WriteLine("Enter TargetId: ");
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
                case "Update":
                    break;
                case "present_according_to_the_id":
                    String print=" ";
                    print += $"Enter what would you like to do\n";
                    print += $"1 to present a station according to his id.\n";
                    print += $"2 to present a drone according to his id.\n";
                    print += $"1 to present a customer according to his id.\n";
                    print += $"1 to present a parcel according to his id.\n";
                    int choice;
                    int.TryParse(Console.ReadLine(), out choice);
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("Enter the base station id\n");
                            int id;
                            int.TryParse(Console.ReadLine(), out id);
                            Console.WriteLine(DalObject.PrintBaseStation(id).ToString());
                            break;
                        case 2:
                            Console.WriteLine("Enter the base drone id\n");
                            int idDrone;
                            int.TryParse(Console.ReadLine(), out idDrone);
                            Console.WriteLine(DalObject.PrintBaseStation(idDrone).ToString()); 
                            break;
                        case 3:
                            Console.WriteLine("Enter the base customer id\n");
                            int idCustomer;
                            int.TryParse(Console.ReadLine(), out idCustomer);
                            Console.WriteLine(DalObject.PrintBaseStation(idCustomer).ToString());
                            break;
                        case 4:
                            Console.WriteLine("Enter the base parcel id\n");
                            int idParcel;
                            int.TryParse(Console.ReadLine(), out idParcel);
                            Console.WriteLine(DalObject.PrintBaseStation(idParcel).ToString());
                            break;
                    }
                    break;
                case "present_the_whole_list":
                    
                    break;
            }
        }
    }
}
