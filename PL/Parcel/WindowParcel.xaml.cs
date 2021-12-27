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
    /// Interaction logic for WindowParcel.xaml
    /// </summary>
    public partial class WindowParcel : Window
    {
        Parcel mainParcel = new();

        private bool _close { get; set; } = false;
        BL.BL ibl;
        private WindowParcels windowParcels;

        /// <summary>
        /// constructer-adds a new Parcel   
        /// </summary>
        /// <param name="bl">the accses to IBL</param>
        /// <param name="_windowParcels">the window with all the Parcel</param>
        public WindowParcel(BL.BL bl, WindowParcels _windowParcels)
        {
            InitializeComponent();
            ibl = bl;
            windowParcels = _windowParcels;
            AddGrid.Visibility = Visibility.Visible;
            DataContext = mainParcel;
            WeightComboBox.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            PriorityComboBox.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            addDeletButton.Content = "Add the Parcel";
            addDeletButton.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// constructer-updates the drone that the mouse clicked twice on
        /// </summary>
        /// <param name="bl">the accses to IBL</param>
        /// <param name="_windowParcels">the window with all the drones</param>
        /// <param name="i">the diffrence between the constractor of add to the constractor of update</param>
        public WindowParcel(BL.BL bl, WindowParcels _windowParcels, int i = 0)
        {
            ibl = bl;
            InitializeComponent();
            windowParcels = _windowParcels;
            ActionseGrid.Visibility = Visibility.Visible;
            Buttens.Visibility = Visibility.Visible;
            mainParcel = ibl.GetParcel(windowParcels.selectedParcel.Id);//returnes the drone that the mouce clicked twise on
            DataContext = mainParcel;//to connect between the text box and the data
            if (mainParcel.Scheduled == null)
            {
                addDeletButton.Content = "Delet the parcel";
                addDeletButton.Visibility = Visibility.Visible;
            }
            if (mainParcel.Scheduled != null)//if the parcel  has a drone 
            {
                if (mainParcel.Delivered == null && mainParcel.PickedUp == null)
                {
                    DeliverPickUpButton.Content = "Pick up";
                }
                if (mainParcel.Delivered == null && mainParcel.PickedUp != null)
                {
                    DeliverPickUpButton.Content = "Deliver";
                }

                DroneInParcel.Visibility = Visibility.Visible;//show the gride of the parcels drone
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

        private void addDeletButton_Click(object sender, RoutedEventArgs e)
        {
            if (addDeletButton.Content == "Add the Parcel")
            {
                try
                {
                    if (mainParcel.Id == default || mainParcel.Sender == default || mainParcel.Recepter == default)
                        throw new MissingInfoException("No information entered for this drone");
                    ibl.AddParcel(mainParcel);
                    mainParcel = ibl.GetParcel(mainParcel.Id);
                    windowParcels.Parcels.Add(ibl.GetParcels().First(i => i.Id == mainParcel.Id));
                    MessageBoxResult messageBoxResult = MessageBox.Show("The parcel has been added successfully \n" + mainParcel.ToString());
                    _close = true;
                    Close();
                }
                catch (FailedToAddException ex)
                {
                    var message = MessageBox.Show("Failed to add the parcel: " + ex.GetType().Name + "\n" + ex.Message + "\n" + "Woul'd you like to try agein?\n", "Error",
                        MessageBoxButton.YesNo, MessageBoxImage.Error);
                    switch (message)
                    {
                        case MessageBoxResult.Yes:
                            IDBoxA.Text = "";
                            SenderBoxA.Text = "";
                            RecepterBoxA.Text = "";
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
                    var message = MessageBox.Show("Failed to add the parcel: " + ex.GetType().Name + "\n" + ex.Message + "\n" + "Woul'd you like to try agein?\n", "Error",
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
                    var message = MessageBox.Show("Failed to add the parcel: " + ex.GetType().Name + "\n" + ex.Message + "\n" + "Woul'd you like to try agein?\n", "Error",
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
            else
            {
                ibl.DeletParcel(mainParcel);
            }
        }

        private void DeliverPickUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (DeliverPickUpButton.Content == "Pick up")
            {
                try
                {
                    ibl.PickUpParcel(mainParcel.ParecelDrone.Id);
                }
                catch (FailToUpdateException ex)
                {

                    
                }
            }
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

        Customer send = new();
        private void SenderButten_Click(object sender, RoutedEventArgs e)
        {
            send = ibl.GetCustomer(int.Parse(SenderBoxA.Text));
            WindowCustomers windowCustomers = new WindowCustomers(ibl);
            windowCustomers.selectedCustomer.Id = send.Id;
            new WindowCustomer(ibl, windowCustomers, 0).Show();
        }
        Customer recepter = new();

        private void RecepterButten_Click(object sender, RoutedEventArgs e)
        {
            recepter = ibl.GetCustomer(int.Parse(SenderBoxA.Text));
            WindowCustomers windowCustomers = new WindowCustomers(ibl);
            windowCustomers.selectedCustomer.Id = recepter.Id;
            new WindowCustomer(ibl, windowCustomers, 0).Show();
        }
    }
}
