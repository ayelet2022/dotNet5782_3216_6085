using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using BO;
using System.ComponentModel;

namespace PL
{
    /// <summary>
    /// Interaction logic for WindowCustomer.xaml
    /// </summary>
    public partial class WindowCustomer : Window
    {
        Customer mainCustomer = new();
        private bool _close { get; set; } = false;
        BL.BL ibl;
        private WindowCustomers windowCustomers;

        /// <summary>
        /// constructer-adds a new customer   
        /// </summary>
        /// <param name="bl">the accses to IBL</param>
        /// <param name="_windowCustomers">the window with all the Customers</param>
        public WindowCustomer(BL.BL bl, WindowCustomers _windowCustomers)
        {
            InitializeComponent();
            ibl = bl;
            windowCustomers = _windowCustomers;
            IDBoxA.IsReadOnly = false;
            NameBoxA.IsReadOnly = false;
            PhoneBoxA.IsReadOnly = false;
            LatitudeBoxA.IsReadOnly = false;
            LongitudeBoxA.IsReadOnly = false;
            DataContext = mainCustomer;
        }

        /// <summary>
        /// constructer-updates the Customer that the mouse clicked twice on
        /// </summary>
        /// <param name="bl">the accses to IBL</param>
        /// <param name="_windowCustomers">the window with all the Customers</param>
        /// <param name="i">the diffrence between the constractor of add to the constractor of update</param>
        public WindowCustomer(BL.BL bl, WindowCustomers _windowCustomers, int i = 0)
        {
            ibl = bl;
            InitializeComponent();
            windowCustomers = _windowCustomers;
            Buttens.Visibility = Visibility.Visible;
            mainCustomer = ibl.GetCustomer(windowCustomers.selectedCustomer.Id);//returnes the drone that the mouce clicked twise on
            DataContext = mainCustomer;//to connect between the text box and the data
            ParcelsFromCustomer.Visibility = Visibility.Visible;
            public ObservableCollection<CustomerList> Customers;
        public CustomerList selectedCustomer = new();
    }
        //changes the buttens content according to the drone statuse
        //if (mainDrone.Status == (BO.DroneStatus)DroneStatus.Available)//if the drone is available
        //{
        //    ChargeDrone.Visibility = Visibility.Visible;
        //    ChangeStatusDrone.Visibility = Visibility.Visible;
        //    ChargeDrone.Content = "Send drone to charging";
        //    ChangeStatusDrone.Content = "Send drone to delievery";
        //}
        //if (mainDrone.Status == (BO.DroneStatus)DroneStatus.InFix)//if the drone is in charge
        //{
        //    ChargeDrone.Content = "Release drone from charging";
        //    ChangeStatusDrone.Visibility = Visibility.Hidden;
        //}
        //else//meens the drone is in delivery
        //{
        //    //if the drone is in delivery and the parcel in the drone isnt on the way
        //    if (mainDrone.Status == (BO.DroneStatus)DroneStatus.Delivery && mainDrone.ParcelInTransfer.StatusParcel == false)
        //    {
        //        ChargeDrone.Visibility = Visibility.Hidden;
        //        ChangeStatusDrone.Content = "Pick up parcel";
        //    }
        //    //if the drone is in delivery and the parcel in the drone is on the way
        //    if (mainDrone.Status == (BO.DroneStatus)DroneStatus.Delivery && mainDrone.ParcelInTransfer.StatusParcel == true)
        //    {
        //        ChargeDrone.Visibility = Visibility.Hidden;
        //        ChangeStatusDrone.Content = "Supply parcel";
        //    }
        //}
    }

        /// <summary>
        /// when the butten add was prest and the new drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButten_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mainCustomer.Id == default || mainCustomer.Name == default|| mainCustomer.Phone==default)
                    throw new MissingInfoException("No information entered for this Customer");
                ibl.AddCustomer(mainCustomer);
                mainCustomer = ibl.GetCustomer(mainCustomer.Id);
                windowCustomers.Customers.Add(ibl.GetCustomers().First(i => i.Id == mainCustomer.Id));
                MessageBoxResult messageBoxResult = MessageBox.Show("The drone has been added successfully \n" + mainCustomer.ToString());
                _close = true;
                Close();
            }
            catch (FailedToAddException ex)
            {
                var message = MessageBox.Show("Failed to add the customer: " + ex.GetType().Name + "\n" + ex.Message + "\n" + "Woul'd you like to try agein?\n", "Error",
                    MessageBoxButton.YesNo, MessageBoxImage.Error);
                switch (message)
                {
                    case MessageBoxResult.Yes:
                        IDBoxA.Text = "";
                        NameBoxA.Text = "";
                        PhoneBoxA.Text = "";
                        break;
                    case MessageBoxResult.No:
                        _close = true;
                        Close();
                        break;
                    default:
                        break;
                }
            }
            catch (InvalidInputException ex)
            {
                var message = MessageBox.Show("Failed to add the drone: " + ex.GetType().Name + "\n" + ex.Message + "\n" + "Woul'd you like to try agein?\n", "Error",
                    MessageBoxButton.YesNo, MessageBoxImage.Error);
                switch (message)
                {
                    case MessageBoxResult.Yes:
                        IDBoxA.Text = "";
                        break;
                    case MessageBoxResult.No:
                        _close = true;
                        Close();
                        break;
                    default:
                        break;
                }
            }
            catch (MissingInfoException ex)
            {
                var message = MessageBox.Show("Failed to add the drone: " + ex.GetType().Name + "\n" + ex.Message + "\n" + "Woul'd you like to try agein?\n", "Error",
                    MessageBoxButton.YesNo, MessageBoxImage.Error);
                switch (message)
                {
                    case MessageBoxResult.Yes:
                        break;
                    case MessageBoxResult.No:
                        _close = true;
                        Close();
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// when the butten of update was clicked and updates the drones model according to what was enterd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (NameBoxAc.Text == null)//if the model name was not enterd
                //    throw new MissingInfoException($"The customer model: {mainCustomer.Name} is incorrect, the drone was not added.\n");
                ibl.UpdateCustomer(mainCustomer.Id,NameBoxA.Text,PhoneBoxA.Text);//change the drones model according to what was enterd
                int index = windowCustomers.Customers.IndexOf(windowCustomers.selectedCustomer);//fineds the index of the drone that we wanted to update
                if(NameBoxA.Text!=default)
                    windowCustomers.selectedCustomer.Name = NameBoxA.Text;//changes the model of the drone thet was clicked in the drones list
                if (PhoneBoxA.Text != default)
                    windowCustomers.selectedCustomer.Phone = PhoneBoxA.Text;//changes the model of the drone thet was clicked in the drones list
                windowCustomers.Customers[index] = windowCustomers.selectedCustomer;//to update the drone in the list of drones in the main window
                MessageBoxResult messageBoxResult = MessageBox.Show("The Customer has been updateded successfully \n" + mainCustomer.ToString());
            }
            catch (FailToUpdateException ex)
            {
                MessageBox.Show("Failed to update the Customer: " + ex.GetType().Name + "\n" + ex.Message);
            }
            catch (MissingInfoException ex)
            {
                MessageBox.Show("Failed to update the Customer: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }

        /// <summary>
        /// when a butten was clicked-checks what the butten content and act according to that
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void ChangeStatusCustomer_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (mainCustomer.Status == (BO.DroneStatus)DroneStatus.Available)//meens we can only scheduled a parcel to the drone
        //        {
        //            ibl.ScheduledAParcelToADrone(mainDrone.Id);//scheduled a parcel to the drone
        //            ChargeDrone.Visibility = Visibility.Collapsed;
        //            ParcelIT.Visibility = Visibility.Visible;
        //            ChangeStatusDrone.Content = "Pick up parcel";//to change the butten conntact to only be to pick up the parcel
        //        }
        //        //meens we can only pick up the parcel that the drone supposed to delivery
        //        if (mainDrone.Status == (BO.DroneStatus)DroneStatus.Delivery && mainDrone.ParcelInTransfer.StatusParcel == false)
        //        {
        //            ibl.PickUpParcel(mainDrone.Id);//update the drone to pick up the parcel
        //            ChargeDrone.Visibility = Visibility.Collapsed;
        //            ChangeStatusDrone.Content = "Supply parcel";//to change the butten conntact to only be to delivery the parcel
        //        }
        //        //meens we can only pick up the parcel that the drone deliver to delivery
        //        if (mainDrone.Status == (BO.DroneStatus)DroneStatus.Delivery && mainDrone.ParcelInTransfer.StatusParcel == true)
        //        {
        //            ibl.DeliverParcel(mainDrone.Id);//update the drone to deliver the parcel
        //            ParcelIT.Visibility = Visibility.Collapsed;//ther is no parcel in the drone now
        //            ChargeDrone.Visibility = Visibility.Visible;
        //            ChargeDrone.Content = "Send drone to charging";//to change the butten conntact to only be send to drone
        //            ChangeStatusDrone.Content = "Send drone to delievery";//to change the butten conntact to only be delivering a parcel
        //        }
        //        int index = windowDrones.Drones.IndexOf(windowDrones.selectedDrone);//fineds the index of the drone that we wanted to update                                                                                                //fineds the drone that we were updating  index in the list 
        //        windowDrones.Drones[index] = ibl.GetDrones().First(item => item.Id == mainDrone.Id);//updates the drones list
        //        mainDrone = ibl.GetDrone(mainDrone.Id);//updates the main drone
        //        DataContext = mainDrone;//updates all the text box according to what was updated
        //        MessageBoxResult messageBoxResult = MessageBox.Show("The drone has been updateded successfully \n" + mainDrone.ToString());
        //    }
        //    catch (FailToUpdateException ex)
        //    {
        //        MessageBox.Show("Failed to update the drone: " + ex.GetType().Name + "\n" + ex.Message);
        //    }
        //}


        /// <summary>
        /// to close the drones window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            _close = true;
            Close();
        }
        /// <summary>
        /// to only allow to enter int in a text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// to not be able to close the window with the x on the top
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowClose(object sender, CancelEventArgs e)
        {
            if (!_close)
            {
                e.Cancel = true;
                MessageBox.Show("You can't force the window to close");
            }
        }
    }
}
