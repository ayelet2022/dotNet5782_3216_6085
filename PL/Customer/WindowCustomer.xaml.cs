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
        BlApi.IBL ibl;
        public ParcelList selectedparcelFC = new();
        public ParcelList selectedParcelTC = new();
        private WindowCustomers windowCustomers;
        public ObservableCollection<ParcelInCustomer> ParcelFromCusW;
        public ObservableCollection<ParcelInCustomer> ParcelToCusW;
        private WindowParcel windowParcel { get; set; }

        #region CUNSTRACTERS
        public WindowCustomer(BlApi.IBL bl, WindowParcel _windowParcel, int id, Parcel _parcel) : this(bl, null, id)
        {
            windowParcel = _windowParcel;
        }
        /// <summary>
        /// constructer-adds a new customer   
        /// </summary>
        /// <param name="bl">the accses to IBL</param>
        /// <param name="_windowCustomers">the window with all the Customers</param>
        public WindowCustomer(BlApi.IBL bl, WindowCustomers _windowCustomers)
        {
            InitializeComponent();
            ibl = bl;
            windowCustomers = _windowCustomers;
            AddGrid.Visibility = Visibility.Visible;
            UpdateGride.Visibility = Visibility.Collapsed;
            UpdateAddButton.Content = "Add a customer";
            mainCustomer.CustomerLocation = new();
            DataContext = mainCustomer;
        }

        /// <summary>
        /// constructer-updates the Customer that the mouse clicked twice on
        /// </summary>
        /// <param name="bl">the accses to IBL</param>
        /// <param name="_windowCustomers">the window with all the Customers</param>
        /// <param name="i">the diffrence between the constractor of add to the constractor of update</param>
        public WindowCustomer(BlApi.IBL bl, WindowCustomers _windowCustomers, int i = 0)
        {
            ibl = bl;
            InitializeComponent();
            windowCustomers = _windowCustomers;
            AddGrid.Visibility = Visibility.Collapsed;
            UpdateGride.Visibility = Visibility.Visible;
            AddParcel.Visibility = Visibility.Visible;
            UpdateAddButton.Content = "Update the customer";
            mainCustomer.CustomerLocation = new();
            if (i == 0)
                mainCustomer = ibl.GetCustomer(windowCustomers.selectedCustomer.Id);//returnes the drone that the mouce clicked twise on
            else
                mainCustomer = ibl.GetCustomer(i);
            DataContext = mainCustomer;//to connect between the text box and the data
            ParcelFromCusW = new ObservableCollection<ParcelInCustomer>();
            List<ParcelInCustomer> parcelInCustomerFromCus = mainCustomer.ParcelsFromCustomers.ToList();
            if (parcelInCustomerFromCus.Count() != 0)
            {
                ParcelFromCusLable.Visibility = Visibility.Visible;
                ParcelFromCusListView.Visibility = Visibility.Visible;
                foreach (var item in parcelInCustomerFromCus)
                    ParcelFromCusW.Add(item);
                ParcelFromCusListView.ItemsSource = ParcelFromCusW;
            }
            ParcelToCusW = new ObservableCollection<ParcelInCustomer>();
            List<ParcelInCustomer> parcelInCustomerToCus = mainCustomer.ParcelsToCustomers.ToList();
            if (parcelInCustomerToCus.Count() != 0)
            {
                ParcelToCusLable.Visibility = Visibility.Visible;
                ParcelToCusListView.Visibility = Visibility.Visible;
                foreach (var item in parcelInCustomerToCus)//to fet and shoe all the drones
                    ParcelToCusW.Add(item);
                ParcelToCusListView.ItemsSource = ParcelToCusW;//to show all the drones 
            }
        }
        #endregion

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
                    MessageBoxResult messageBoxResult = MessageBox.Show("The customer has been added successfully \n" + mainCustomer.ToString());
                    _close = true;
                    Close();
                }
                #region CATCH
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
                    var message = MessageBox.Show("Failed to add the customer: " + ex.GetType().Name + "\n" + ex.Message + "\n" + "Woul'd you like to try agein?\n", "Error",
                        MessageBoxButton.YesNo, MessageBoxImage.Error);
                    switch (message)
                    {
                        case MessageBoxResult.Yes:
                            IDBoxA.Text = "";
                            PhoneBoxA.Text = "";
                            NameBoxA.Text = "";
                            LongitudeBoxA.Text = "";
                            LatitudeBoxA.Text = "";
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
                    var message = MessageBox.Show("Failed to add the customer: " + ex.GetType().Name + "\n" + ex.Message + "\n" + "Woul'd you like to try agein?\n", "Error",
                        MessageBoxButton.YesNo, MessageBoxImage.Error);
                    switch (message)
                    {
                        case MessageBoxResult.Yes:
                            IDBoxA.Text = "";
                            PhoneBoxA.Text = "";
                            NameBoxA.Text = "";
                            LongitudeBoxA.Text = "";
                            LatitudeBoxA.Text = "";
                            break;
                        case MessageBoxResult.No:
                            _close = true;
                            Close();
                            break;
                        default:
                            break;
                    }
                }
                #endregion
            }
            else//meens to update the customer
            {
                try
                {
                    AddGrid.Visibility = Visibility.Collapsed;
                    UpdateGride.Visibility = Visibility.Visible;
                    if (NameBoxA.Text == "" || mainCustomer.Phone == ""|| NameBoxA.Text == default || mainCustomer.Phone == default)
                        throw new MissingInfoException("No information entered for this Customer");
                    ibl.UpdateCustomer(mainCustomer.Id, NameBoxA.Text, PhoneBoxA.Text);//change the drones model according to what was enterd
                    mainCustomer = ibl.GetCustomer(mainCustomer.Id);
                    int index = -1;
                    if (windowCustomers != null)
                    {
                        index = windowCustomers.Customers.IndexOf(windowCustomers.selectedCustomer);//fineds the index of the drone that we wanted to update
                        windowCustomers.selectedCustomer.Name = mainCustomer.Name;//changes the model of the drone thet was clicked in the drones list
                        windowCustomers.selectedCustomer.Phone = mainCustomer.Phone;//changes the model of the drone thet was clicked in the drones list
                        windowCustomers.Customers[index] = windowCustomers.selectedCustomer;//to update the drone in the list of drones in the main window
                    }
                    else
                        windowParcel.MyRefresh();
                    MessageBoxResult messageBoxResult = MessageBox.Show("The Customer has been updateded successfully \n" + mainCustomer.ToString());
                }
                #region CATCH
                catch (FailToUpdateException ex)
                {
                    var message = MessageBox.Show("Failed to update the customer: " + ex.GetType().Name + "\n" + ex.Message + "\n" + "Woul'd you like to try agein?\n", "Error",
                        MessageBoxButton.YesNo, MessageBoxImage.Error);
                    switch (message)
                    {
                        case MessageBoxResult.Yes:
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
                catch (MissingInfoException ex)
                {
                    var message = MessageBox.Show("Failed to update the customer: " + ex.GetType().Name + "\n" + ex.Message + "\n" + "Woul'd you like to try agein?\n", "Error",
                      MessageBoxButton.YesNo, MessageBoxImage.Error);
                    switch (message)
                    {
                        case MessageBoxResult.Yes:
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
                catch (NotFoundInputException ex)
                {
                    var message = MessageBox.Show("Failed to update the customer: " + ex.GetType().Name + "\n" + ex.Message + "\n" + "Woul'd you like to try agein?\n", "Error",
                      MessageBoxButton.YesNo, MessageBoxImage.Error);
                    switch (message)
                    {
                        case MessageBoxResult.Yes:
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
                #endregion

            }
        }

        /// <summary>
        /// shows the all parcel that was clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParcelToCusListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelInCustomer parcel = (ParcelInCustomer)ParcelToCusListView.SelectedItem;//the drone that the mous double clicked on
            new WindowParcel(ibl, this, parcel.Id, ParcelToCusListView.SelectedIndex,mainCustomer.Id).Show();//to show the all the details of the drone and to be able to updae him
        }

        /// <summary>
        /// shows the all parcel that was clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParcelFromCusListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelInCustomer parcel = (ParcelInCustomer)ParcelFromCusListView.SelectedItem;
            new WindowParcel(ibl, this, parcel.Id, ParcelFromCusListView.SelectedIndex, mainCustomer.Id).Show();//to show the all the details of the drone and to be able to updae him
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

        private void AddParcel_Click(object sender, RoutedEventArgs e)
        {
            WindowParcels windowParcels = new WindowParcels(ibl);
            new WindowParcel(ibl, windowParcels).Show();
        }
        private void ImageF_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement framework = sender as FrameworkElement;
            selectedparcelFC = framework.DataContext as ParcelList;
            try
            {
                ibl.PickUpParcel(ibl.GetDrones().First(item => item.NumOfParcelOnTheWay == selectedparcelFC.Id).Id);
                ParcelInCustomer parcelInCustomer = new()
                {
                    Id = selectedparcelFC.Id,
                    Weight = selectedparcelFC.Weight,
                    Priority = selectedparcelFC.Priority,
                    Status = selectedparcelFC.ParcelStatus,
                    SenderOrRecepter = new()
                    {
                        Id = ibl.GetParcel(selectedparcelFC.Id).Sender.Id,
                        Name = selectedparcelFC.SenderName
                    }
                };
                ParcelFromCusW.Remove(parcelInCustomer);
                parcelInCustomer.Status = BO.ParcelStatus.pickup;
                ParcelFromCusW.Add(parcelInCustomer);
                MessageBoxResult messageBoxResult = MessageBox.Show("The parcel has been picked up successfully \n" + selectedparcelFC.ToString());
            }
            catch (FailToUpdateException ex)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("The parcel was not picked up \n" + selectedparcelFC.ToString());
            }

        }

        private void ImageT_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement framework = sender as FrameworkElement;
            ParcelList parcel= framework.DataContext as ParcelList;
            selectedParcelTC = parcel;
            if (selectedparcelFC.ParcelStatus == BO.ParcelStatus.delivery)
                throw new FailToUpdateException("the parcel is already deliverd");
            try
            {
                ibl.DeliverParcel(ibl.GetDrones().First(item => item.NumOfParcelOnTheWay == selectedparcelFC.Id).Id);
                ParcelInCustomer parcelInCustomer = new()
                {
                    Id = selectedparcelFC.Id,
                    Weight = selectedparcelFC.Weight,
                    Priority = selectedparcelFC.Priority,
                    Status = selectedparcelFC.ParcelStatus,
                    SenderOrRecepter = new()
                    {
                        Id = ibl.GetParcel(selectedparcelFC.Id).Recepter.Id,
                        Name = selectedparcelFC.SenderName
                    }
                };
                ParcelFromCusW.Remove(parcelInCustomer);
                MessageBoxResult messageBoxResult = MessageBox.Show("The parcel has been deliverd successfully \n" + selectedparcelFC.ToString());
            }
            catch (FailToUpdateException ex)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("The parcel was not deliverd  \n" + selectedparcelFC.ToString());
            }

        }
    }
}
