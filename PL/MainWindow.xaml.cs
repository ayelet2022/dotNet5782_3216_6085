using BO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BlApi.IBL bl = BL.BlFactory.GetBl();
        /// <summary>
        /// the constractor of main window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            options.Visibility = Visibility.Visible;
        }

        private void newCusButten_Click(object sender, RoutedEventArgs e)
        {
            WindowCustomers windowCustomers = new WindowCustomers(bl);
            new WindowCustomer(bl, windowCustomers).Show();
        }
        private void managerButten_Click(object sender, RoutedEventArgs e)
        {
            ManagerButten.IsEnabled = false;
            PasswordBox.Visibility = Visibility.Visible;
            SignInM.Visibility = Visibility.Visible;
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
                if (idTB.Text == default || idTB.Text == "")
                    throw new MissingInfoException("No id was entered");
                Customer customer = bl.GetCustomer(int.Parse(idTB.Text));
                WindowCustomers windowCustomers = new(bl);
                idTB.Text = null;
                new WindowCustomer(bl, windowCustomers, customer.Id).Show();
            }
            catch (MissingInfoException ex)
            {
                MessageBox.Show("Failed to find the customer: " + ex.GetType().Name + "\n" + ex.Message);
                idTB.Text = null;

            }
            catch (NotFoundInputException ex)
            {
                MessageBox.Show("Failed to find the customer: " + ex.GetType().Name + "\n" + ex.Message);
                idTB.Text = null;

            }

        }

        private void newCusButten_Click_1(object sender, RoutedEventArgs e)
        {
            WindowCustomers windowCustomers = new(bl);
            new WindowCustomer(bl, windowCustomers).Show();
        }

        private void GoBackButten_Click(object sender, RoutedEventArgs e)
        {
            manager.Visibility = Visibility.Collapsed;
            options.Visibility = Visibility.Visible;
        }

        private void SignInM_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PasswordBox.Password == "010203")
                {
                    options.Visibility = Visibility.Collapsed;
                    manager.Visibility = Visibility.Visible;
                    ManagerButten.IsEnabled = true;
                    PasswordBox.Password = null;
                    SignInM.Visibility = Visibility.Collapsed;
                    PasswordBox.Visibility = Visibility.Collapsed;
                }
                else
                {
                    throw new InvalidInputException("Worng password");
                }
            }
            catch (InvalidInputException ex)
            {
                var message = MessageBox.Show(ex.Message + "\n" + "Woul'd you like to try agein?\n", "Error",
                            MessageBoxButton.YesNo, MessageBoxImage.Error);
                switch (message)
                {
                    case MessageBoxResult.Yes:
                        PasswordBox.Password = null;
                        break;
                    case MessageBoxResult.No:
                        SignInM.Visibility = Visibility.Collapsed;
                        ManagerButten.IsEnabled = true;
                        PasswordBox.Password = null;
                        PasswordBox.Visibility = Visibility.Collapsed;
                        break;
                    default:
                        break;
                }
            }

        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
