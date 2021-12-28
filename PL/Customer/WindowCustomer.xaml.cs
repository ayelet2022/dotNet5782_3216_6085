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
        public ParcelList selectedparcelFC = new();
        public CustomerList selectedParcelTC = new();
        private WindowCustomers windowCustomers;
        public ObservableCollection<ParcelInCustomer> ParcelFromCusW;
        public ObservableCollection<ParcelInCustomer> ParcelToCusW;


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
            AddGrid.Visibility=Visibility.Visible;
            UpdateGride.Visibility = Visibility.Collapsed;
            UpdateAddButton.Content = "Add a customer";
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
            AddGrid.Visibility = Visibility.Collapsed;
            UpdateGride.Visibility = Visibility.Visible;
            UpdateAddButton.Content = "Update the customer";
            mainCustomer = ibl.GetCustomer(windowCustomers.selectedCustomer.Id);//returnes the drone that the mouce clicked twise on
            DataContext = mainCustomer;//to connect between the text box and the data
            ParcelFromCusW = new ObservableCollection<ParcelInCustomer>();
            List<ParcelInCustomer> parcelInCustomerFromCus = mainCustomer.ParcelsFromCustomers.ToList();
            if (parcelInCustomerFromCus != null)
            {
                ParcelFromCusListView.Visibility = Visibility.Visible;
                foreach (var item in parcelInCustomerFromCus)
                    ParcelFromCusW.Add(item);
                ParcelFromCusListView.ItemsSource = ParcelFromCusW;
            }
            ParcelToCusW = new ObservableCollection<ParcelInCustomer>();
            List<ParcelInCustomer> parcelInCustomerToCus = mainCustomer.ParcelsToCustomers.ToList();
            if (parcelInCustomerToCus != null)
            {
                ParcelToCusListView.Visibility = Visibility.Visible;
                foreach (var item in parcelInCustomerToCus)//to fet and shoe all the drones
                    ParcelToCusW.Add(item);
                ParcelToCusListView.ItemsSource = ParcelToCusW;//to show all the drones 
            }
        }

        /// <summary>
        /// when the butten of update was clicked and updates the drones model according to what was enterd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (UpdateAddButton.Content == "Add a customer")
            {
                try
                {
                    if (mainCustomer.Id == default || mainCustomer.Name == default || mainCustomer.Phone == default)
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
            else
            {
                try
                {
                    AddGrid.Visibility = Visibility.Collapsed;
                    UpdateGride.Visibility = Visibility.Visible;
                    ibl.UpdateCustomer(mainCustomer.Id, NameBoxA.Text, PhoneBoxA.Text);//change the drones model according to what was enterd
                    int index = windowCustomers.Customers.IndexOf(windowCustomers.selectedCustomer);//fineds the index of the drone that we wanted to update
                    if (NameBoxA.Text != default)
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
        }

        /// <summary>
        /// shows the all parcel that was clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParcelToCusListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelInCustomer parcel = (ParcelInCustomer)ParcelFromCusListView.SelectedItem;//the drone that the mous double clicked on
            WindowParcels windowParcels = new WindowParcels(ibl);
            new WindowParcel(ibl, windowParcels, parcel.Id).Show();//to show the all the details of the drone and to be able to updae him
        }

        /// <summary>
        /// shows the all parcel that was clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParcelFromCusListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelInCustomer parcel = (ParcelInCustomer)ParcelFromCusListView.SelectedItem;
            WindowParcels windowParcels = new WindowParcels(ibl);
            new WindowParcel(ibl, windowParcels, parcel.Id).Show();//to show the all the details of the drone and to be able to updae him
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
