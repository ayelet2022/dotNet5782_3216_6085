using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for WindowStations.xaml
    /// </summary>
    public partial class WindowStations : Window
    {
        private bool _close { get; set; } = false;
        BlApi.IBL ibl;
        public ObservableCollection<BaseStationList> Stations;
        public BaseStationList selectedStation = new();
        public WindowStations(BlApi.IBL bl)
        {
            InitializeComponent();
            ibl = bl;
            Stations = new ObservableCollection<BaseStationList>();
            List<BaseStationList> stations = ibl.GetBaseStations().ToList();
            stations.OrderBy(item => item.Id);
            foreach(var stat in stations)Stations.Add(stat);
            stationList.ItemsSource = Stations;//to show all the stations
        }
        private void stationList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedStation = (BaseStationList)stationList.SelectedItem;//the drone that the mous double clicked on
            new WindowStation(ibl, this, 0).Show();//to show the all the details of the drone and to be able to updae him
        }

        private void buttenAdd_Click(object sender, RoutedEventArgs e)
        {
            new WindowStation(ibl, this).Show();
        }

        private void WindowClose(object sender, CancelEventArgs e)
        {
            if (!_close)
            {
                e.Cancel = true;
                MessageBox.Show("You can't force the window to close");
            }
        }

        public void MyRefresh()
        {
            Stations = new ObservableCollection<BaseStationList>();
            List<BaseStationList> stations = ibl.GetBaseStations().ToList();
            foreach (var stat in stations) Stations.Add(stat);
            stationList.ItemsSource = Stations;//to show all the stations
        }
        /// <summary>
        /// to close the window of the drones list 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseWDS_Click(object sender, RoutedEventArgs e)
        {
            _close = true;
            Close();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            MyRefresh();
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement framework = sender as FrameworkElement;
            selectedStation = framework.DataContext as BaseStationList;
            try
            {
                ibl.DeleteStation(selectedStation.Id);
                Stations.Remove(selectedStation);
                MessageBoxResult messageBoxResult = MessageBox.Show("The station has been deleted successfully \n" + selectedStation.ToString());
            }
            catch (BO.ItemIsDeletedException ex)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("The station was not deleted \n" + selectedStation.ToString());
            }

        }
    }
}
