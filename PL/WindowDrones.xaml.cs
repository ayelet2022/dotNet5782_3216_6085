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
using IBL.BO;
namespace PL
{
    /// <summary>
    /// Interaction logic for WindowDrones.xaml
    /// </summary>
    public partial class WindowDrones : Window
    {
        IBL.IBL ibl;
        public WindowDrones(IBL.IBL bl)
        {
            InitializeComponent();
            ibl = bl;
            DronesListView.ItemsSource = ibl.GetDrones();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selector();
        }
        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selector();
        }
        private void Selector()
        {
            if (StatusSelector.SelectedItem == null && WeightSelector.SelectedItem != null)
                DronesListView.ItemsSource = ibl.GetDrones(item => item.MaxWeight == (WeightCategories)WeightSelector.SelectedItem);
            if (StatusSelector.SelectedItem != null && WeightSelector.SelectedItem == null)
                DronesListView.ItemsSource = ibl.GetDrones(item => item.Status == (DroneStatus)StatusSelector.SelectedItem);
            if (StatusSelector.SelectedItem != null && WeightSelector.SelectedItem != null)
                DronesListView.ItemsSource = ibl.GetDrones(item => item.Status == (DroneStatus)StatusSelector.SelectedItem && item.MaxWeight == (WeightCategories)WeightSelector.SelectedItem);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            new WindowDrone(ibl).Show();
        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DronesListView_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            new WindowDrone((DroneList)DronesListView.SelectedItem).Show();
        }
    }
}
