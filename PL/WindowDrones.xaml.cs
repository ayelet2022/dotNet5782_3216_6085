﻿using System;
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
        public WindowDrones(IBL.IBL bl)
        {
            InitializeComponent();
            ibl = bl;
            DronesListView.ItemsSource = ibl.GetDrones();
        }
        IBL.IBL ibl;
        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            

        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
        }
    }
}
