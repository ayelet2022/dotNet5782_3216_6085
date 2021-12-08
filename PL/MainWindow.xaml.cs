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
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IBL.IBL bl = new BL.BL();

        /// <summary>
        /// the constractor of main window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// to show the all list of the drones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showDroneButton_Click(object sender, RoutedEventArgs e)
        {
            new WindowDrones(bl).Show();//to show the all drones
        }
    }
}
