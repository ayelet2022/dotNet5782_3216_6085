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
        BL.BL ibl;
        public ObservableCollection<BaseStationList> Stations;
        public BaseStationList selectedStation = new();
        public WindowStations(BL.BL bl)
        {
            InitializeComponent();
            ibl = bl;
            Stations = new ObservableCollection<BaseStationList>();
            List<BaseStationList> stations = ibl.GetBaseStations().ToList();
            foreach (var item in stations)//to fet and shoe all the drones
                Stations.Add(item);
            stationList.ItemsSource = Stations;//to show all the drones 
        }
        private void stationList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedStation = (BaseStationList)stationList.SelectedItem;//the drone that the mous double clicked on
            new WindowStation(ibl, this, stationList.SelectedIndex).Show();//to show the all the details of the drone and to be able to updae him
        }
    }
}
