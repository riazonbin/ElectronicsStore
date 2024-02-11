using Microsoft.Win32;
using ElectronicsStore.ADOModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
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

namespace ElectronicsStore.Controls.ManagerControls
{
    /// <summary>
    /// Interaction logic for CreatingDishManagerControl.xaml
    /// </summary>
    public partial class CreatingDishManagerControl : UserControl
    {
        private ADOModel.MenuItem _menuItem;
        private bool isEdit = false;
        Grid _gridMain;
        public CreatingDishManagerControl(ADOModel.MenuItem menuItem, Grid GridMain)
        {
            _gridMain = GridMain;
            InitializeComponent();

            _menuItem = menuItem;

            if (_menuItem != null)
            {
                isEdit = true;
            }
            else
            {
                _menuItem = new ADOModel.MenuItem();
            }
        }

        private void ChooseImageButton(object sender, RoutedEventArgs e)
        {
            var window = new OpenFileDialog();

            if (window.ShowDialog() != true)
            {
                ModernWpf.MessageBox.Show("Изображение не выбрано");
                return;
            }

            var byteArray = File.ReadAllBytes(window.FileName);
            _menuItem.Image = byteArray;
            BindingOperations.GetBindingExpressionBase(ImageDish, Image.SourceProperty).UpdateTarget();

        }

        private void UserControlLoaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = _menuItem;
            BtnDeleteMenuItem.Visibility = isEdit ? Visibility.Visible : Visibility.Collapsed;
            cbDiscounts.ItemsSource = App.Connection.Discount.ToList().OrderBy(x => x.Value);
            cbMenuItemType.ItemsSource = App.Connection.MenuItemType.ToList().OrderBy(x => x.Name);
        }

        private void SaveDishButton(object sender, RoutedEventArgs e)
        {
            if (!isEdit)
            {
                var res = App.Connection.MenuItem.FirstOrDefault(x => x.Name == _menuItem.Name);
                if (res != null)
                {
                    ModernWpf.MessageBox.Show("Блюдо с таким названием уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            App.Connection.MenuItem.AddOrUpdate(_menuItem);
            App.Connection.SaveChanges();


            ModernWpf.MessageBox.Show("Блюдо успешно сохранено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);


            _gridMain.Children.Clear();
            UserControl usc = new DishesManagerControl(_gridMain);
            _gridMain.Children.Add(usc);
        }

        private void GetBackButton(object sender, RoutedEventArgs e)
        {
            _gridMain.Children.Clear();
            UserControl usc = new DishesManagerControl(_gridMain);
            _gridMain.Children.Add(usc);
        }

        private void UserControlUnloaded(object sender, RoutedEventArgs e)
        {
            if (isEdit)
            {
                App.Connection.Entry(_menuItem).Reload();
            }
        }

        private void DeleteMenuItem(object sender, RoutedEventArgs e)
        {
            _menuItem.IsMarkedForDeletion = true;
            App.Connection.MenuItem.AddOrUpdate(_menuItem);
            App.Connection.SaveChanges();
            ModernWpf.MessageBox.Show("Блюдо успешно удалено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);



            _gridMain.Children.Clear();
            UserControl usc = new DishesManagerControl(_gridMain);
            _gridMain.Children.Add(usc);
        }
    }
}
