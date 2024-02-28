using ElectronicsStore.ADO;
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

namespace ElectronicsStore.Controls.ClientControls
{
    /// <summary>
    /// Interaction logic for CartUserControl.xaml
    /// </summary>
    public partial class BasketClientControl : UserControl
    {
        public BasketClientControl()
        {
            InitializeComponent();

            RefreshData();
        }

        private void PlusButtonClick(object sender, RoutedEventArgs e)
        {
            var menuItemId = (int)((Button)sender).Tag;

            Basket newUserCart = new Basket()
            {
                Product_Id = menuItemId,
                User_Id = App.currentUser.Id,
            };
            App.Connection.Basket.Add(newUserCart);
            App.Connection.SaveChanges();

            dgCart.Items.Refresh();
        }

        private void MinusButtonClick(object sender, RoutedEventArgs e)
        {
            var productId = (int)((Button)sender).Tag;

            App.Connection.Basket.Remove(App.Connection.Basket.FirstOrDefault(x => x.Product_Id == productId));
            App.Connection.SaveChanges();

            RefreshData();
        }

        private void RemoveAllButtonClick(object sender, RoutedEventArgs e)
        {
            var menuItemId = (int)((Button)sender).Tag;


            App.Connection.Basket.RemoveRange(App.Connection.Basket.Where(x => x.Product_Id == menuItemId).ToList());
            App.Connection.SaveChanges();

            RefreshData();
        }

        private void RefreshData()
        {
            dgCart.ItemsSource = App.Connection.Basket.Where(x => x.User_Id == App.currentUser.Id).GroupBy(x => x.Product_Id).ToList();
            dgCart.Items.Refresh();
        }

        private void UserControlLoaded(object sender, RoutedEventArgs e)
        {
            if(dgCart.Items.Count == 0)
            {
                approveOrderButton.Visibility = Visibility.Collapsed;
            }
        }

        private void ApproveOrderButtonClick(object sender, RoutedEventArgs e)
        {
            // add new order
            Order newOrder = new Order()
            {
                OrderStartDate = DateTime.Now,
                OrderStatus_Id = 1,
                User_Id = App.currentUser.Id,
            };


            foreach(Basket item in App.Connection.Basket.Where(x => x.User_Id == App.currentUser.Id).ToList())
            {
                OrderContent content = new OrderContent()
                {
                    Product_Id = (int)item.Product_Id,
                    
                };
                newOrder.OrderContent.Add(content);
            }
            App.Connection.Order.Add(newOrder);

            // clear cart
            App.Connection.Basket.RemoveRange(App.Connection.Basket.Where(x => x.User_Id == App.currentUser.Id));
            App.Connection.SaveChanges();

            RefreshData();

            ModernWpf.MessageBox.Show("Заказ успешно оформлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

        }
    }
}
