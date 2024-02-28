using ElectronicsStore.ADO;
using MaterialDesignThemes.Wpf;
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

namespace ElectronicsStore.Pages
{
    /// <summary>
    /// Interaction logic for RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void RegistrateButton(object sender, RoutedEventArgs e)
        {

            if (tbFirstname.Text == "" || tbLastname.Text == "" || tbLogin.Text == "" || tbPassword.Password == "")
            {
                ModernWpf.MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            User newUser = new User()
            {
                Password = tbPassword.Password,
                Firstname = tbFirstname.Text,
                Lastname = tbLastname.Text,
                Login = tbLogin.Text,
                Role_Id = 2 
            };

            if (App.Connection.User.FirstOrDefault(x => x.Login == newUser.Login) != null)
            {
                ModernWpf.MessageBox.Show(messageBoxText: "Пользователь с таким логином уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            App.Connection.User.Add(newUser);
            App.Connection.SaveChanges();

            ModernWpf.MessageBox.Show("Пользователь зарегистрирован!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.Navigate(new AuthorizationPage());

        }
    }
}
