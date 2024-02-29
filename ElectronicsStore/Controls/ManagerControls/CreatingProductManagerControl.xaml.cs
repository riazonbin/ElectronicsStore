using Microsoft.Win32;
using ElectronicsStore.ADO;
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
    public partial class CreatingProductManagerControl : UserControl
    {
        private Product _product;
        private bool isEdit = false;
        Grid _gridMain;
        public CreatingProductManagerControl(Product menuItem, Grid GridMain)
        {
            _gridMain = GridMain;
            InitializeComponent();

            _product = menuItem;

            if (_product != null)
            {
                isEdit = true;
            }
            else
            {
                _product = new Product();
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
            _product.Image = byteArray;
            BindingOperations.GetBindingExpressionBase(ImageDish, Image.SourceProperty).UpdateTarget();

        }

        private void UserControlLoaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = _product;
            BtnDeleteMenuItem.Visibility = isEdit ? Visibility.Visible : Visibility.Collapsed;
            cbMenuItemType.ItemsSource = App.Connection.ProductType.ToList().OrderBy(x => x.Name);
        }

        private void SaveDishButton(object sender, RoutedEventArgs e)
        {
            if(tbName.Text == "" || cbMenuItemType.SelectedIndex == -1)
            {
                ModernWpf.MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!isEdit)
            {
                var res = App.Connection.Product.FirstOrDefault(x => x.Name == _product.Name);
                if (res != null)
                {
                    ModernWpf.MessageBox.Show("Товар с таким названием уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            App.Connection.Product.AddOrUpdate(_product);
            App.Connection.SaveChanges();


            ModernWpf.MessageBox.Show("Товар успешно сохранен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);


            _gridMain.Children.Clear();
            UserControl usc = new ProductsCatalogUserControl(_gridMain);
            _gridMain.Children.Add(usc);
        }

        private void GetBackButton(object sender, RoutedEventArgs e)
        {
            _gridMain.Children.Clear();
            UserControl usc = new ProductsCatalogUserControl(_gridMain);
            _gridMain.Children.Add(usc);
        }

        private void UserControlUnloaded(object sender, RoutedEventArgs e)
        {
            if (isEdit)
            {
                App.Connection.Entry(_product).Reload();
            }
        }

        private void DeleteMenuItem(object sender, RoutedEventArgs e)
        {
            _product.IsMarkedForDeletion = true;
            App.Connection.Product.AddOrUpdate(_product);
            App.Connection.SaveChanges();
            ModernWpf.MessageBox.Show("Товар успешно удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            _gridMain.Children.Clear();
            UserControl usc = new ProductsCatalogUserControl(_gridMain);
            _gridMain.Children.Add(usc);
        }
    }
}
