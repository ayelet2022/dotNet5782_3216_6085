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
    /// Interaction logic for WindowDrone.xaml
    /// </summary>
    public partial class WindowDrone : Window
    {
        StatusAndWeight _StatusAndWeight;
        Drone mainDrone = new();
        private bool _close { get; set; } = false;
        BlApi.IBL ibl;
        private WindowDrones windowDrones;
        
        /// <summary>
        /// constructer-adds a new drone   
        /// </summary>
        /// <param name="bl">the accses to IBL</param>
        /// <param name="_windowDrones">the window with all the drones</param>
        public WindowDrone(BlApi.IBL bl, WindowDrones _windowDrones)
        {
            InitializeComponent();
            ibl = bl;
            _StatusAndWeight = new();
            windowDrones = _windowDrones;
            AddGrid.Visibility = Visibility.Visible;
            DataContext = mainDrone;
            weightA.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            IdStation.ItemsSource = bl.GetBaseStations(x => x.EmptyCharges != 0).Select(s => s.Id);//the first basestation that the drone was charging in
        }

        /// <summary>
        /// constructer-updates the drone that the mouse clicked twice on
        /// </summary>
        /// <param name="bl">the accses to IBL</param>
        /// <param name="_windowDrones">the window with all the drones</param>
        /// <param name="i">the diffrence between the constractor of add to the constractor of update</param>
        public WindowDrone(BlApi.IBL bl, WindowDrones _windowDrones, int i=0)
        {
            ibl = bl;
            InitializeComponent();
            windowDrones = _windowDrones;
            ActionseGrid.Visibility=Visibility.Visible;
            Buttens.Visibility = Visibility.Visible;
            if (i == 0)
                mainDrone = ibl.GetDrone(windowDrones.selectedDrone.Id);//returnes the drone that the mouce clicked twise on
            else
                mainDrone = ibl.GetDrone(i);
            DataContext = mainDrone;//to connect between the text box and the data
            if(mainDrone.ParcelInTransfer!=null)//if the drone has a parcel 
            {
                ParcelIT.Visibility = Visibility.Visible;//the window with the enfo of the parcel in the drone
            }
            //changes the buttens content according to the drone statuse
            if (mainDrone.Status == (BO.DroneStatus)DroneStatus.Available)//if the drone is available
            {
                ChargeDrone.Visibility = Visibility.Visible;
                ChangeStatusDrone.Visibility = Visibility.Visible;
                ChargeDrone.Content = "Send drone to charging";
                ChangeStatusDrone.Content = "Send drone to delievery";
            }
            if (mainDrone.Status == (BO.DroneStatus)DroneStatus.InFix)//if the drone is in charge
            {
                ChargeDrone.Content = "Release drone from charging";
                ChangeStatusDrone.Visibility = Visibility.Hidden;
            }
            else//meens the drone is in delivery
            {
                //if the drone is in delivery and the parcel in the drone isnt on the way
                if (mainDrone.Status == (BO.DroneStatus)DroneStatus.Delivery && mainDrone.ParcelInTransfer.StatusParcel == false)
                {
                    ChargeDrone.Visibility = Visibility.Hidden;
                    ChangeStatusDrone.Content = "Pick up parcel";
                }
                //if the drone is in delivery and the parcel in the drone is on the way
                if (mainDrone.Status == (BO.DroneStatus)DroneStatus.Delivery && mainDrone.ParcelInTransfer.StatusParcel == true)
                {
                    ChargeDrone.Visibility = Visibility.Hidden;
                    ChangeStatusDrone.Content = "Supply parcel";
                }
            }
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
                if (mainDrone.Id == default || mainDrone.Model == default)
                    throw new MissingInfoException("No information entered for this drone");
                if (IdStation.SelectedItem == null)
                    throw new MissingInfoException("No station was entered for this drone");
                ibl.AddDrone(mainDrone, (int)IdStation.SelectedItem);
                _StatusAndWeight.status = mainDrone.Status;
                _StatusAndWeight.weight = mainDrone.MaxWeight;
                mainDrone = ibl.GetDrone(mainDrone.Id);
                if (windowDrones.Drones.ContainsKey(_StatusAndWeight))
                    windowDrones.Drones[_StatusAndWeight].Add(ibl.GetDrones().First(i => i.Id == mainDrone.Id));
                else
                    windowDrones.Drones.Add(_StatusAndWeight, ibl.GetDrones().Where(i => i.Id == mainDrone.Id).ToList());
                windowDrones.Selector();
                MessageBoxResult messageBoxResult = MessageBox.Show("The drone has been added successfully \n" + mainDrone.ToString());
                _close = true;
                Close();
            }
            catch(FailedToAddException ex)
            {
                var message = MessageBox.Show("Failed to add the drone: " + ex.GetType().Name + "\n" + ex.Message+"\n"+"Woul'd you like to try agein?\n","Error",
                    MessageBoxButton.YesNo, MessageBoxImage.Error);
                switch (message)
                {
                    case MessageBoxResult.Yes:
                        IdBoxA.Text = "";
                        ModelBoxA.Text = "";
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
                        IdBoxA.Text = "";
                        break;
                    case MessageBoxResult.No:
                        _close = true;
                        Close();
                        break;
                    default:
                        break;
                }
            }
            catch(MissingInfoException ex)
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
                if (ModelBoxAc.Text == null)//if the model name was not enterd
                    throw new MissingInfoException($"The drones model: {mainDrone.Model} is incorrect, the drone was not added.\n");
                string oldName = windowDrones.selectedDrone.Model;
                ibl.UpdateDrone(mainDrone, ModelBoxAc.Text);//change the drones model according to what was enterd
                windowDrones.Selector();
                MessageBoxResult messageBoxResult = MessageBox.Show("The drone has been updateded successfully \n" + mainDrone.ToString());
            }
            catch (FailToUpdateException ex)
            {
                MessageBox.Show("Failed to update the drone: " + ex.GetType().Name + "\n" + ex.Message);
            }
            catch (MissingInfoException ex)
            {
                MessageBox.Show("Failed to update the drone: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }

        /// <summary>
        /// when a butten was clicked-checks what the butten content and act according to that
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeStatusDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mainDrone.Status == ( BO.DroneStatus)DroneStatus.Available)//meens we can only scheduled a parcel to the drone
                {
                    ibl.ScheduledAParcelToADrone(mainDrone.Id);//scheduled a parcel to the drone
                    ChargeDrone.Visibility = Visibility.Collapsed;
                    ParcelIT.Visibility = Visibility.Visible;
                    ChangeStatusDrone.Content = "Pick up parcel";//to change the butten conntact to only be to pick up the parcel
                }
                //meens we can only pick up the parcel that the drone supposed to delivery
                if (mainDrone.Status == (BO.DroneStatus)DroneStatus.Delivery && mainDrone.ParcelInTransfer.StatusParcel == false)
                {
                    ibl.PickUpParcel(mainDrone.Id);//update the drone to pick up the parcel
                    ChargeDrone.Visibility = Visibility.Collapsed;
                    ChangeStatusDrone.Content = "Supply parcel";//to change the butten conntact to only be to delivery the parcel
                }
                //meens we can only pick up the parcel that the drone deliver to delivery
                if (mainDrone.Status == (BO.DroneStatus)DroneStatus.Delivery && mainDrone.ParcelInTransfer.StatusParcel == true)
                {
                    ibl.DeliverParcel(mainDrone.Id);//update the drone to deliver the parcel
                    ParcelIT.Visibility = Visibility.Collapsed;//ther is no parcel in the drone now
                    ChargeDrone.Visibility = Visibility.Visible;
                    ChargeDrone.Content = "Send drone to charging";//to change the butten conntact to only be send to drone
                    ChangeStatusDrone.Content = "Send drone to delievery";//to change the butten conntact to only be delivering a parcel
                }                                                                                                //fineds the drone that we were updating  index in the list 
                DataContext = ibl.GetDrone(mainDrone.Id);
                windowDrones.Selector();
                mainDrone = ibl.GetDrone(mainDrone.Id);
                MessageBoxResult messageBoxResult = MessageBox.Show("The drone has been updateded successfully \n" + mainDrone.ToString());
            }
            catch (FailToUpdateException ex)
            {
                MessageBox.Show("Failed to update the drone: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }

        /// <summary>
        /// to send the drone to charging or to relese him from charging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChargeDrone_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                //checks what is the butten content and acts according to that
                if ((string)ChargeDrone.Content == "Send drone to charging")//meens we want to send the drone to charging
                {
                    ibl.SendDroneToCharging(mainDrone.Id);//to send the drone to charging
                    ChangeStatusDrone.Visibility = Visibility.Hidden;
                    ChargeDrone.Content = "Release drone from charging";//meens we can now reles the drone fron charging
                }
                else
                    if ((string)ChargeDrone.Content == "Release drone from charging")//meens we want to relese the drone from charging
                { 
                    ibl.FreeDroneFromeCharger(mainDrone.Id);//to free the drone from chargimg
                    ChangeStatusDrone.Visibility = Visibility.Visible;
                    ChargeDrone.Content = "Send drone to charging";//we can send now the drone to charging 
                    ChangeStatusDrone.Content = "Send drone to delievery";//we can send the drone to deliver
                }
                DataContext = ibl.GetDrone(mainDrone.Id);
                mainDrone = ibl.GetDrone(mainDrone.Id);
                windowDrones.Selector();
                MessageBoxResult messageBoxResult = MessageBox.Show("The drone has been updateded successfully \n" + mainDrone.ToString());
            }
            catch (FailToUpdateException ex)
            {
                MessageBox.Show("Failed to update the drone: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }

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
