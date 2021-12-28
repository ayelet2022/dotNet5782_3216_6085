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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BO;
namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BL.BL bl= BL.BlFactory.GetBl();
        /// <summary>
        /// the constractor of main window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        private void newCusButten_Click(object sender, RoutedEventArgs e)
        {
            WindowCustomers windowCustomers = new(bl);
            new WindowCustomer(bl, windowCustomers).Show();
        }
        private void managerButten_Click(object sender, RoutedEventArgs e)
        {
            options.Visibility = Visibility.Collapsed;
            manager.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// to show the all list of the drones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void dronesButten_Click(object sender, RoutedEventArgs e)
        {
            new WindowDrones(bl).Show();
        }

        private void parcelsButten_Click(object sender, RoutedEventArgs e)
        {
            new WindowParcels(bl).Show();
        }

        private void customersButten_Click(object sender, RoutedEventArgs e)
        {
            new WindowCustomers(bl).Show();
        }

        private void stationsButten_Click(object sender, RoutedEventArgs e)
        {
            new WindowStations(bl).Show();
        }

        private void OldCus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (idTB.Text == default|| idTB.Text == "")
                    throw new MissingInfoException("No id was entered");
                Customer customer = bl.GetCustomer(int.Parse(idTB.Text));
                WindowCustomers windowCustomers = new(bl);
                new WindowCustomer(bl, windowCustomers,customer.Id).Show();
            }
            catch(MissingInfoException ex)
            {
                MessageBox.Show("Failed to find the customer: " + ex.GetType().Name + "\n" + ex.Message);
            }
            catch (NotFoundInputException ex)
            {
                MessageBox.Show("Failed to find the customer: " + ex.GetType().Name + "\n" + ex.Message);
            }
            
        }

        private void newCusButten_Click_1(object sender, RoutedEventArgs e)
        {
            WindowCustomers windowCustomers = new(bl);
            new WindowCustomer(bl, windowCustomers).Show();
        }
    }
}
