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
    /// Interaction logic for WindowDrone.xaml
    /// </summary>
    public partial class WindowDrone : Window
    {
        Drone drone = new();
        IBL.IBL ibl;
        public WindowDrone(IBL.IBL bl)
        {
            InitializeComponent();
            ibl = bl;
            add.Visibility = Visibility.Visible;
            weight1.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            drone.Id = (int)IdBox.DataContext;
            drone.Model = (string)ModelBox.DataContext;
        }
        public WindowDrone(Drone drone)
        {
            
        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void weight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            drone.MaxWeight = (WeightCategories)status1.SelectedItem;
        }

        private void addButten_Click(object sender, RoutedEventArgs e)
        {
            ibl.AddDrone(drone, (int)IdStation.DataContext);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
