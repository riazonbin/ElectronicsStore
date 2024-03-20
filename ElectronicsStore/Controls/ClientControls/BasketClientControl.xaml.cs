using ElectronicsStore.ADO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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

            ReloadData();
        }

        private void ReloadData()
        {
            LvBasketItems.ItemsSource = App.Connection.Basket.Where(x => x.User_Id == App.CurrentUser.Id).ToList(
                );

            var totalCount = App.Connection.Basket.Where(x => x.User_Id == App.CurrentUser.Id).Sum(x => x.Count);

            if (totalCount > 0)
            {
                TbTotalCount.Text = $"Товары({totalCount})";
                TbTotalPrice.Text = $"{App.Connection.Basket.Where(x => x.User_Id == App.CurrentUser.Id).Sum(x => x.Count * x.Product.Price)} ₽";
                BtnCreateNewOrder.Visibility = Visibility.Visible;
            }
            else
            {
                TbTotalCount.Text = "Корзина пуста";
                TbTotalPrice.Text = "";
                BtnCreateNewOrder.Visibility = Visibility.Collapsed;
            }

        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {

            var basketId = (int)((Button)sender).Tag;

            var basket = App.Connection.Basket.FirstOrDefault(x => x.Id == basketId);

            var product = basket.Product;


            basket.Count++;

            App.Connection.Basket.AddOrUpdate(basket);
            App.Connection.SaveChanges();

            ReloadData();
        }

        private void RemoveItemButtonClick(object sender, RoutedEventArgs e)
        {
            var basketId = (int)((Button)sender).Tag;

            var basket = App.Connection.Basket.FirstOrDefault(x => x.Id == basketId);

            var product = basket.Product;


            if (basket.Count > 1)
            {
                basket.Count--;
                App.Connection.Basket.AddOrUpdate(basket);
            }
            else
            {
                App.Connection.Basket.Remove(basket);
            }

            App.Connection.SaveChanges();

            ReloadData();
        }

        private void CreateOrderButtonClick(object sender, RoutedEventArgs e)
        {
            var startOrderStatus = App.Connection.OrderStatus.FirstOrDefault(x => x.Id == 1);

            var order = new Order
            {
                OrderStatus = startOrderStatus,
                User_Id = App.CurrentUser.Id,
                OrderStartDate = DateTime.Now,
            };

            var userBasketList = App.Connection.Basket.Where(x => x.User_Id == App.CurrentUser.Id).ToList();

            foreach (var item in userBasketList)
            {
                var orderContent = new OrderContent
                {
                    Order = order,
                    Product_Id = item.Product_Id,
                    Count = item.Count,
                };
                order.OrderContent.Add(orderContent);
            }


            App.Connection.Order.AddOrUpdate(order);
            App.Connection.Basket.RemoveRange(userBasketList);
            App.Connection.SaveChanges();

            App.Connection.Entry(order).Reload();

            SnackbarSuccess.MessageQueue?.Enqueue($"Заказ успешно создан!",
            null, null, null, false,
            true, TimeSpan.FromSeconds(2));

            ReloadData();

        }
    }
}
