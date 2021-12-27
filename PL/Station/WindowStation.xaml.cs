using System;
using System.Collections.Generic;
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
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for WindowStation.xaml
    /// </summary>
    public partial class WindowStation : Window
    {
        BaseStation station = new();
        private bool _close { get; set; } = false;
        BlApi.IBL ibl;
        private WindowStations windowStations;

        /// <summary>
        /// constructer-updates the station that the mouse clicked twice on
        /// </summary>
        /// <param name="bl">the accses to IBL</param>
        /// <param name="_windowStations">the window with all the stations</param>
        /// <param name="i">the diffrence between the constractor of add to the constractor of update</param>
        public WindowStation(BlApi.IBL bl, WindowStations _windowStations, int i = 0)
        {
            InitializeComponent();
            ibl = bl;
            windowStations = _windowStations;
            updateGrid.Visibility = Visibility.Visible;
            station.BaseStationLocation = new();
            station = ibl.GetBaseStation(windowStations.selectedStation.Id);
            DataContext = station;
            if (station.DronesInCharge.Count() != 0)
            {
                labelDronesInStation.Visibility = Visibility.Visible;
                listDronesInStation.Visibility = Visibility.Visible;
                listDronesInStation.ItemsSource = station.DronesInCharge;
            }
            buttenAddUpdate.Content = "UPDATE";
        }

        /// <summary>
        /// constructer-adds a new station   
        /// </summary>
        /// <param name="bl">the accses to IBL</param>
        /// <param name="_windowStations">the window with all the stations</param>
        public WindowStation(BlApi.IBL bl, WindowStations _windowStations)
        {
            InitializeComponent();
            ibl = bl;
            windowStations = _windowStations;
            addGrid.Visibility = Visibility.Visible;
            station.BaseStationLocation = new();
            DataContext = station;
            buttenAddUpdate.Content = "ADD";
        }

        private void listDronesInStation_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            WindowDrones windowDrones = new WindowDrones(ibl);
            //Drone drone=ibl.GetDrone(listDronesInStation.SelectedItem)
            windowDrones.selectedDrone = (DroneList)listDronesInStation.SelectedItem;
            new WindowDrone(ibl, windowDrones, 0).Show();
        }

        private void buttenAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (buttenAddUpdate.Content == "ADD")
                {
                    if (station.Id == default || station.EmptyCharges == default || station.Name == default || station.BaseStationLocation.Latitude == default || station.BaseStationLocation.Longitude == default)
                        throw new MissingInfoException("No information entered for this station");
                    ibl.AddBaseStation(station);
                    station = ibl.GetBaseStation(station.Id);
                    int key = station.EmptyCharges;
                    if (windowStations.Stations.ContainsKey(key))
                        windowStations.Stations[key].Add(ibl.GetBaseStations().First(i => i.Id == station.Id));
                    else
                        windowStations.Stations.Add(key, ibl.GetBaseStations().Where(i => i.Id == station.Id).ToList());
                    windowStations.MyRefresh();
                    MessageBoxResult messageBoxResult = MessageBox.Show("The station has been added successfully \n" + station.ToString());
                    _close = true;
                    Close();
                }
                else
                {
                    try
                    {
                        //StationListWindow.CurrentStation.Name = NameTxtUp.Text;//updating drone name
                        //StationListWindow.CurrentStation.AvailableChargeSlots = int.Parse(ChargeSlotsTxtUp.Text);
                        ibl.UpdateStation(station.Id, nameTB.Text, int.Parse(avaiChargesTB.Text));
                        //updates all the text box according to what was updated
                        windowStations.selectedStation.Name = nameTB.Text;
                        windowStations.selectedStation.EmptyCharges = int.Parse(avaiChargesTB.Text);
                        windowStations.MyRefresh();
                        MessageBoxResult messageBoxResult = MessageBox.Show("The station has been updateded successfully \n" + station.ToString());
                    }
                    catch (InvalidInputException ex)
                    {
                        MessageBox.Show("Failed to update the station: " + ex.GetType().Name + "\n" + ex.Message);
                    }
                }
            }
            catch (FailedToAddException ex)
            {
                var message = MessageBox.Show("Failed to add the station: " + ex.GetType().Name + "\n" + ex.Message + "\n" + "Woul'd you like to try agein?\n", "Error",
                    MessageBoxButton.YesNo, MessageBoxImage.Error);
                switch (message)
                {
                    case MessageBoxResult.Yes:
                        idTB.Text = "";
                        avaiChargesTB.Text = "";
                        nameTB.Text = "";
                        longtitudeTB.Text = "";
                        latitudeTB.Text = "";
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
                var message = MessageBox.Show("Failed to add the station: " + ex.GetType().Name + "\n" + ex.Message + "\n" + "Woul'd you like to try agein?\n", "Error",
                    MessageBoxButton.YesNo, MessageBoxImage.Error);
                switch (message)
                {
                    case MessageBoxResult.Yes:
                        idTB.Text = "";
                        longtitudeTB.Text = "";
                        latitudeTB.Text = "";
                        avaiChargesTB.Text = "";
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
                var message = MessageBox.Show("Failed to add the station: " + ex.GetType().Name + "\n" + ex.Message + "\n" + "Woul'd you like to try agein?\n", "Error",
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
    }
}
