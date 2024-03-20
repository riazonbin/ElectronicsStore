using ElectronicsStore.ADO;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElectronicsStore.Controls
{
    /// <summary>
    /// Interaction logic for CurrentOrdersUserControl.xaml
    /// </summary>
    public partial class CurrentOrdersUserControl : UserControl
    {
        List<Order> _orders;

        public CurrentOrdersUserControl()
        {
            InitializeComponent();
            App.dispatcherTimer.Tick += new EventHandler((s, e) => UpdateData());
        }


        private void LoadData()
        {
            if (App.CurrentUser.Role.Id == 2)
            {
                _orders = App.Connection.Order.Where(x => x.User_Id == App.CurrentUser.Id && x.OrderStatus.Id != 6).ToList();
                LvOrders.ItemsSource = _orders;
            }
            else
            {
                _orders = App.Connection.Order.Where(x => x.User_Id == App.CurrentUser.Id && x.OrderStatus.Id != 6).ToList();
                LvOrders.ItemsSource = _orders;
            }

            if (LvOrders.Items.Count == 0)
            {
                TbDullOrders.Visibility = Visibility.Visible;
            }
        }

        private void UpdateData()
        {
            if (App.CurrentUser.Role.Id == 2)
            {
                var newData =  App.Connection.Order.Where(x => x.User_Id == App.CurrentUser.Id && x.OrderStatus.Id != 6).ToList();
                if(newData != _orders)
                {
                    _orders = newData;
                }
            }
            else
            {
                var newData = App.Connection.Order.Where(x => x.User_Id == App.CurrentUser.Id && x.OrderStatus.Id != 6).ToList();
                if(newData != _orders)
                {
                    _orders = newData;
                }
            }
        }

        private void UserControlLoaded(object sender, RoutedEventArgs e)
        {
            TbDullOrders.Visibility = Visibility.Collapsed;
            LoadData();
        }

        private void UserControlUnloaded(object sender, RoutedEventArgs e)
        {
            App.dispatcherTimer.Tick -= new EventHandler((s, e1) => LoadData());
        }
    }
}
