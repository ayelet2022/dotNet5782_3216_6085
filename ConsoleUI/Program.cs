using System;
using DAL;
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
                    Choice = Console.ReadLine();
                    switch(Choice)
                    {
                        case "BaseStation":
                            
                            break;
                        case "Drone":

                            break;
                        case "Customer":

                            break;
                        case "Parcel":

                            break;
                    }
                    break;
                case "Update":
                    Choice = Console.ReadLine();
                    switch (Choice)
                    {
                        case "To_pare_a_parcel":

                            break;
                        case "To_pickup_a_parcel":

                            break;
                        case "Deliver_to_a_customer":

                            break;
                        case "send_drone_to_charge":

                            break;
                        case "free_drone_from_charger":

                            break;
                    }
                    break;
                case "present_according_to_the_id":
                    Choice = Console.ReadLine();
                    switch (Choice)
                    {
                        case "BaseStation":

                            break;
                        case "Drone":

                            break;
                        case "Customer":

                            break;
                        case "Parcel":

                            break;
                    }
                    break;
                case "present_the_whole_list":
                    Choice = Console.ReadLine();
                    switch (Choice)
                    {
                        case "BaseStation":

                            break;
                        case "Drone":

                            break;
                        case "Customer":

                            break;
                        case "Parcel":

                            break;
                        case "Parcel_that_weren't_paired":
                            break;
                        case "BaseStation_with_available_charges":
                            break;
                    }
                    break;
                case "Exit":
                    break;
            }
        }
    }
}
