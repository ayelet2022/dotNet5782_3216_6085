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
        public Dictionary<int ,List<BaseStationList>> Stations;
        public BaseStationList selectedStation = new();
        public WindowStations(BlApi.IBL bl)
        {
            InitializeComponent();
            ibl = bl;
            Stations = new Dictionary<int, List<BaseStationList>>();
            Stations = (from item in ibl.GetBaseStations()
                        group item by item.EmptyCharges
                      ).ToDictionary(item => item.Key, item => item.ToList());
            stationList.ItemsSource = Stations.Values.SelectMany(item => item);//to show all the drones 
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

        private void buttenClose_Click(object sender, RoutedEventArgs e)
        {
            _close = true;
            Close();
        }

        public void MyRefresh()
        {
            stationList.ItemsSource = from item in Stations.Values.SelectMany(x => x)
                                      orderby item.EmptyCharges
                                      select item;//to show the all list
        }
    }
}
