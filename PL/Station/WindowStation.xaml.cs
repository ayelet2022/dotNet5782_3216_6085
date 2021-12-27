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
        BL.BL ibl;
        private WindowStations windowStations;

        /// <summary>
        /// constructer-updates the station that the mouse clicked twice on
        /// </summary>
        /// <param name="bl">the accses to IBL</param>
        /// <param name="_windowStations">the window with all the stations</param>
        /// <param name="i">the diffrence between the constractor of add to the constractor of update</param>
        public WindowStation(BL.BL bl, WindowStations _windowStations, int i = 0)
        {
            InitializeComponent();
            ibl = bl;
            windowStations = _windowStations;
            updateGrid.Visibility = Visibility.Visible;
            station = ibl.GetBaseStation(windowStations.selectedStation.Id);
            DataContext = station;
            if (station.DronesInCharge != null)
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
        public WindowStation(BL.BL bl, WindowStations _windowStations)
        {
            InitializeComponent();
            ibl = bl;
            windowStations = _windowStations;
            addGrid.Visibility = Visibility.Visible;
            DataContext = station;
            buttenAddUpdate.Content = "ADD";
        }

        private void dronesInStation_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            WindowDrones windowDrones = new WindowDrones(ibl);
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
                    windowStations.Stations.Add(ibl.GetBaseStations().First(i => i.Id == station.Id));
                    MessageBoxResult messageBoxResult = MessageBox.Show("The station has been added successfully \n" + station.ToString());
                                    _close = true;
                Close();
                }
                else
                {
                    try
                    {
                        ibl.UpdateStation(station.Id, nameTB.Text, int.Parse(avaiChargesTB.Text));
                        int index = windowStations.Stations.IndexOf(windowStations.selectedStation);//fineds the index of the drone that we wanted to update                                                                                                //fineds the drone that we were updating  index in the list 
                        windowStations.Stations[index] = ibl.GetBaseStations().First(item => item.Id == station.Id);//updates the drones list
                        station = ibl.GetBaseStation(station.Id);//updates the main drone
                        DataContext = station;//updates all the text box according to what was updated
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
