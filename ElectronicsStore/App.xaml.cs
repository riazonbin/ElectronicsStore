using ElectronicsStore.ADO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ElectronicsStore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ElectronicsStoreEntities Connection = new ElectronicsStoreEntities();
        public static User CurrentUser;
        public static DispatcherTimer dispatcherTimer = new DispatcherTimer();
    }
}
