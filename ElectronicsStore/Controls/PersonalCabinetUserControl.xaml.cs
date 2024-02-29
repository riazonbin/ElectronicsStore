using ElectronicsStore.ADO;
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
        NavigationService _navService;
        bool IsAdminMode = App.CurrentUser.Role_Id == 1;

        public PersonalCabinetUserControl(NavigationService navService)
        {
            _navService = navService;
            InitializeComponent();

            DataContext = App.CurrentUser;

            //wProfileButton.Content = $"{App.CurrentUser.Lastname[0]}.{App.CurrentUser.Firstname[0]}";
        }

        private void SaveChangesButton(object sender, RoutedEventArgs e)
        {
            if(TbFirstname.Text == "" || TbLastname.Text == "" || TbLogin.Text == "")
            {
                ModernWpf.MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            App.Connection.User.AddOrUpdate(App.CurrentUser);
            App.Connection.SaveChanges();

            ProfileButton.Content = $"{App.CurrentUser.Lastname[0]}.{App.CurrentUser.Firstname[0]}";

            ModernWpf.MessageBox.Show("Пользователь успешно сохранен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void UserControlUnloaded(object sender, RoutedEventArgs e)
        {
            if(App.CurrentUser != null)
            {
                App.Connection.Entry(App.CurrentUser).Reload();
            }
        }
    }
}
