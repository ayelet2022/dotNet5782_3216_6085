using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    /// Interaction logic for WindowCustomers.xaml
    /// </summary>
    public partial class WindowCustomers : Window
    {
        private bool _close { get; set; } = false;
        BlApi.IBL ibl;
        public ObservableCollection<CustomerList> Customers;
        public CustomerList selectedCustomer = new();

        /// <summary>
        /// the constractor of the window
        /// </summary>
        /// <param name="bl"> the accses to the fileds in IBL</param>
        public WindowCustomers(BlApi.IBL bl)
        {
            InitializeComponent();
            ibl = bl;
            Customers = new ObservableCollection<CustomerList>();
            List<CustomerList> customers = ibl.GetCustomers().ToList();
            foreach (var item in customers)//to fet and shoe all the drones
                Customers.Add(item);
            CustomerListView.ItemsSource = Customers;//to show all the customers
            Customers.CollectionChanged += Customers_CollactionChanged;
        }
        private void Customers_CollactionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CustomerListView.Items.Refresh();
        }

        /// <summary>
        /// to add a new drone to the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            new WindowCustomer(ibl, this).Show();//t oadd a new drone to the parcel
        }

        /// <summary>
        /// t opresent the drone that the mous double clicked  on
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedCustomer = (CustomerList)CustomerListView.SelectedItem;//the drone that the mous double clicked on
            new WindowCustomer(ibl, this, 0).Show();//to show the all the details of the drone and to be able to updae him
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
    }
}
