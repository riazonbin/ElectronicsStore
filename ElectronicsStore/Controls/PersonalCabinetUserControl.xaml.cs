using ElectronicsStore.ADOModel;
using ElectronicsStore.Pages;
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

namespace ElectronicsStore.Controls
{
    /// <summary>
    /// Interaction logic for PersonalCabinetUserControl.xaml
    /// </summary>
    public partial class PersonalCabinetUserControl : UserControl
    {
        Button _mainPageProfileButton;
        NavigationService _navService;

        public PersonalCabinetUserControl(Button MainPageProfileButton, NavigationService navService)
        {
            _navService = navService;
            _mainPageProfileButton = MainPageProfileButton;
            InitializeComponent();
            DataContext = App.currentUser;

            ProfileButton.Content = $"{App.currentUser.Lastname[0]}.{App.currentUser.Firstname[0]}";
        }

        private void SaveChangesButton(object sender, RoutedEventArgs e)
        {
            if(TbFirstname.Text == "" || TbLastname.Text == "" || TbLogin.Text == "")
            {
                ModernWpf.MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            App.Connection.User.AddOrUpdate(App.currentUser);
            App.Connection.SaveChanges();

            ProfileButton.Content = $"{App.currentUser.Lastname[0]}.{App.currentUser.Firstname[0]}";
            _mainPageProfileButton.Content = $"{App.currentUser.Lastname[0]}.{App.currentUser.Firstname[0]}";

            ModernWpf.MessageBox.Show("Пользователь успешно сохранен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void UserControlUnloaded(object sender, RoutedEventArgs e)
        {
            if(App.currentUser != null)
            {
                App.Connection.Entry(App.currentUser).Reload();
            }
        }

        private void DeleteUserButtonClick(object sender, RoutedEventArgs e)
        {
            ModernWpf.MessageBox.Show("Пользователь успешно удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            App.Connection.User.Remove(App.currentUser);
            App.Connection.SaveChanges();
            App.currentUser = null;
            _navService.Navigate(new AuthorizationPage());
        }
    }
}
